<template>
  <v-card flat>
    <v-card-title>Assets</v-card-title>
    <v-container>
    <v-card>
        <v-spacer></v-spacer>
        <v-data-table :headers="filteredHeaders"
                      :items="assets"
                      :search="search"
                      class="elevation-1"
                      multi-sort
                      @click:row="showDetails">

            <template v-slot:top>
                <v-toolbar flat>

                    <!-- ADDED AUTOCOMPLETE TO THE MODEL SEARCH -->
                    <v-container fluid align="left">
                        <v-row>
                            <v-col class="pt-6 mt-6" cols="2">
                                <v-label>Filter by ... </v-label>
                            </v-col>
                            <v-col cols="6">
                                <v-select v-model="selectedDatacenter"
                                          :items="datacenters"
                                          item-text="description"
                                          item-value=""
                                          :return-object="false"
                                          label="Datacenter"
                                          placeholder="Select a datacenter or all datacenters"
                                          class="pt-8 pl-4"
                                          @change="datacenterSearch()">
                                </v-select>
                            </v-col>
                        </v-row>
                    </v-container>

                    <v-spacer></v-spacer>

                    <v-btn v-if="permission" color="primary" dark class="mb-2" @click="addItem">Add asset</v-btn>

                    <v-dialog v-model="instructionsDialog" max-width="550px">
                        <v-card>
                            <v-card-title class="justify-center">
                                Click on row for more information about the asset
                            </v-card-title>
                        </v-card>
                    </v-dialog>

                </v-toolbar>
                <v-row>
                    <v-col cols="6">
                        <v-row class="pl-10 pt-1">
                            <v-autocomplete prepend-inner-icon="mdi-magnify"
                                            :items="assets"
                                            :search-input.sync="search"
                                            cache-items
                                            class="mt-3 pt-3"
                                            flat
                                            hide-no-data
                                            hide-details
                                            item-text="vendor"
                                            label="Search by keyword on model and hostname"
                                            single-line
                                            solo-inverted></v-autocomplete>

                        </v-row>
                    </v-col>
                    <v-spacer></v-spacer>
                    <!-- Custom filters; sorts between rack ranges -->
                    <v-col cols="4">
                        <v-row class="mt-4 pt-2">
                            <v-text-field v-model="startRackValue"
                                          placeholder="Start"
                                          type="text"
                                          label="Rack Range"
                                          style="width:0">
                            </v-text-field>

                            <v-text-field v-model="endRackValue"
                                          type="text"
                                          placeholder="End"
                                          style="width:0">
                            </v-text-field>
                        </v-row>
                    </v-col>
                    <v-spacer></v-spacer>
                </v-row>
                

            </template>

            <template v-if="permission" v-slot:item.action="{ item }">
                <v-row>
                    <v-tooltip top>
                        <template v-slot:activator="{ on }">
                            <v-btn icon v-on="on"
                                   @click="editItem(item)">
                                <v-icon medium
                                        class="mr-2">
                                    mdi-pencil
                                </v-icon>
                            </v-btn>
                        </template>

                        <span>Edit Asset</span>
                    </v-tooltip>

                    <v-tooltip top>
                        <template v-slot:activator="{ on }">
                            <v-btn icon v-on="on"
                                   @click="decommissionItem(item)">
                                <v-icon medium
                                        class="mr-2">
                                    mdi-archive-arrow-down
                                </v-icon>
                            </v-btn>
                        </template>

                        <span>Decommission Asset</span>
                    </v-tooltip>

                    <v-tooltip top>
                        <template v-slot:activator="{ on }">
                            <v-btn icon v-on="on"
                                   @click="deleteItem(item)">
                                <v-icon medium
                                        class="mr-2">
                                    mdi-delete
                                </v-icon>
                            </v-btn>
                        </template>

                        <span>Delete Asset</span>
                    </v-tooltip>
                </v-row>
            </template>

            <template v-slot:item.power="{ item }">
                <v-row v-if="item.hasNetworkManagedPower">
                    <v-item-group dense
                                  light
                                  tile>
                        <v-btn color ="green lighten-1" 
                               @click="turnOn(item)"
                               x-small
                               width="30%"
                               min-width="30px"
                               depressed>
                            ON
                        </v-btn>
                        <v-btn color ="red lighten-1" 
                               @click="turnOff(item)"
                               min-width="30px"
                               x-small
                               width="30%"
                               depressed>
                            OFF
                        </v-btn>
                        <v-btn color = "grey lighten-2"
                               x-small
                               min-width="40px"
                               width="30%"
                               @click="cycle(item)"
                               depressed>
                            Cycle
                        </v-btn>
                    </v-item-group>
                </v-row>
            </template>

            <template v-slot:no-data>
                <v-btn color="primary" @click="initialize">Refresh</v-btn>
            </template>
        </v-data-table>
    </v-card>
    </v-container>
  </v-card>
</template>

<script>
  export default {
    components: {
    },
    inject: ['assetRepository','modelRepository','userRepository', 'datacenterRepository'],
    data() {
        return {
          selectedDatacenter: 'All Datacenters',
        // Filter values.
        startRackValue: '',
        endRackValue: '',
        datacenters: [],
        instructionsDialog: false,
        loading: true,
        search: '',

        // Table data.
        headers: [
          { text: 'Model Vendor', value: 'vendor' },
          { text: 'Model Number', value: 'modelNumber', },
          { text: 'Hostname', value: 'hostname' },
          { text: 'Datacenter', value: 'datacenter'},
          { text: 'Rack', value: 'rack', filter: this.rackFilter },
          { text: 'Rack U', value: 'rackPosition', },
          { text: 'Owner Username', value: 'owner' },
          { text: 'Power', value: 'power', sortable: false },
          { text: 'Actions', value: 'action', sortable: false },
        ],
        assets: [],
        models: [],
          defaultItem: {
            id: '',
            datacenter: '',
            modelId: '',
            hostname: '',
            rackId: '',
            rackPosition: '',
            ownerId: '',
            comment: '',
            poweredOn: false,
        },
          detailItem: {
            id: '',
            datacenter: '',
            modelId: '',
            hostname: '',
            rackId: '',
            rackPosition: '',
            ownerId: '',
            comment: '',
        },
        editing: false,
      }},
    computed: {
        permission() {
            return this.$store.getters.hasAssetPermission || this.$store.getters.isAdmin
        },
        filteredHeaders() {
            return (this.permission) ? this.headers : this.headers.filter(h => h.text !== "Actions" && h.text !== "Power")
        },
    },
    watch: {
      instructionsDialog (val) {
        val || this.closeDetail()
      },
    },

    async created() {
      this.initialize()
    },
  
    methods: {
        async initialize() {
            this.assets = await this.assetRepository.list();
            this.models = await this.modelRepository.list();
            this.users = await this.userRepository.list();

            this.datacenters = await this.datacenterRepository.list();
                var datacenter = {
                    description: "All Datacenters",
                    name: "All",
                }
            this.datacenters.push(datacenter);

            this.loading = false;

        },
        deleteItem (item) {
        this.editing = true;
        confirm('Are you sure you want to delete this asset?') && this.assetRepository.delete(item)
                    .then(async () => {
                        await this.initialize();
                    })
        },
        decommissionItem (item) {
        this.editing = true;
        confirm('Are you sure you want to decommission this asset?')// && this.assetRepository.decommission(item)
                    .then(async () => {
                        /*eslint-disable*/
                        console.log(item);
                        await this.initialize();
                    })
        },
        async datacenterSearch() {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);
                this.assets = await this.assetRepository.list(searchDatacenter.id); 
            },
        editItem(item) {
            this.editing = true;
            this.$router.push({ name: 'asset-edit', params: { id: item.id } })
        },
        addItem() {
            this.$router.push({ name: 'asset-new' })
        },
        turnOn(item) {
            /*eslint-disable*/
            this.editing = true;
            var powerState = {
                // 0 is on
                action: 0,
            };
            confirm('Are you sure you would like to turn on this asset?') && this.assetRepository.postPowerState(item.id, powerState)
                .then(async () => {
                    console.log(item);
                    await this.initialize();
                })
        },
        turnOff(item) {
            this.editing = true;
            var powerState = {
                // 1 is off
                action: 1,
            };
            confirm('Are you sure you would like to power off this asset?') && this.assetRepository.postPowerState(item.id, powerState)
                .then(async () => {
                    console.log(item);
                    await this.initialize();
                })
        },
        cycle(item) {
            this.editing = true;
            var ret = {
                // 2 is cycle
                action: 2,
            };
            confirm('Are you sure you would like to cycle this asset?') && this.assetRepository.postPowerState(item.id, powerState)
                .then(async () => {
                    console.log(item);
                    await this.initialize();
                })
        },
        showInstructions() {
            this.instructionsDialog = true;
        },
        closeDetail() {
            this.instructionsDialog = false;
        },
        showDetails(item) {
            if (!this.editing) {
            /*eslint-disable*/
                console.log(item);
                this.$router.push({ name: 'asset-details', params: {id: item.id } })
            }
            this.editing = false;
      },

      /**
       * Filter for calories column.
       * @param value Value to be tested; in this case the rack value.
       * @returns {boolean} based on the start and end rack value inputs 
       */
      rackFilter(value) {
        // If this filter has no value we just skip the entire filter.
        if (!this.startRackValue && !this.endRackValue) {
          return true;
        // If only one filter has a value, leans entirely on that one filter
        } else if (!this.endRackValue) {
          return value.toLowerCase() >= this.startRackValue.toLowerCase();
        } else if (!this.startRackValue) {
          return value.toLowerCase() <= this.endRackValue.toLowerCase();
        }  
        // Check if the current loop value (The rack value)
        // is between the rack values inputted
        return value.toLowerCase() >= this.startRackValue.toLowerCase()
                && value.toLowerCase() <= this.endRackValue.toLowerCase();
      },
    
    },
  }
</script>
