<template>
  <v-card flat>
    <v-card-title>Assets</v-card-title>
    <v-container>
    <v-card>
        <v-spacer></v-spacer>
        <v-data-table :headers="filteredHeaders"
                      :items="assets"
                      :search="search"
                      class="pa-10"
                      multi-sort
                      @click:row="showDetails">

            <template v-slot:top>
                <v-toolbar flat>
                    <!-- ADDED AUTOCOMPLETE TO THE MODEL SEARCH -->
                    <v-container fluid align="left">
                        <v-row>
                            <v-col cols="6">
                                <v-row>
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
                                <v-row class="mt-4 pt-6">
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
                    </v-container>

                    <v-spacer></v-spacer>

                    <v-btn v-if="admin" color="primary" dark class="mb-2" @click="addItem">Add asset</v-btn>

                    <v-dialog v-model="instructionsDialog" max-width="550px">
                        <v-card>
                            <v-card-title class="justify-center">
                                Click on row for more information about the asset
                            </v-card-title>
                        </v-card>
                    </v-dialog>

                </v-toolbar>


            </template>

            <template v-if="admin" v-slot:item.action="{ item }">
                <v-row>
                    <v-icon medium
                            class="mr-2"
                            @click="editItem(item)">mdi-pencil</v-icon>
                    <v-icon medium
                            class="mr-2"
                            @click="deleteItem(item)">mdi-delete</v-icon>
                </v-row>
            </template>

            <template v-slot:item.power="{ item }">
                <v-row v-if="item.hasNetworkManagedPower">
                    <v-item-group dense
                                  light
                                  tile>
                        <v-btn color ="green lighten-1" 
                               @click="turnOn(item)"
                               small
                               width="30%"
                               min-width="30px"
                               depressed>
                            ON
                        </v-btn>
                        <v-btn color ="red lighten-1" 
                               @click="turnOff(item)"
                               min-width="30px"
                               small
                               width="30%"
                               depressed>
                            OFF
                        </v-btn>
                        <v-btn color = "grey lighten-2"
                               small
                               min-width="60px"
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
    import Auth from "../auth"

  export default {
    components: {
    },
    inject: ['assetRepository','modelRepository','userRepository'],
    data() {
      return {
        // Filter values.
        startRackValue: '',
        endRackValue: '',

        instructionsDialog: false,
        loading: true,
        search: '',

        // Table data.
        headers: [
          { text: 'Model Vendor', value: 'vendor' },
          { text: 'Model Number', value: 'modelNumber', },
          { text: 'Hostname', value: 'hostname' },
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
        deleting: false,
        editing: false,
        powering: false,
      }},
    computed: {
        admin() {
            return Auth.isAdmin()
        },
        filteredHeaders() {
            return (this.admin) ? this.headers : this.headers.filter(h => h.text !== "Actions")
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
      async initialize () {
        this.assets = await this.assetRepository.list();
            /*eslint-disable*/
            console.log(this.assets);
        this.models = await this.modelRepository.list();
        this.users = await this.userRepository.list();

        this.loading = false;

        },
        deleteItem (item) {
        this.deleting = true;
        confirm('Are you sure you want to delete this item?') && this.assetRepository.delete(item)
                    .then(async () => {
                        await this.initialize();
                    })
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
            this.powering = true;
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
            this.powering = true;
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
            this.powering = true;
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
            if (!this.editing && !this.deleting && !this.powering) {
            /*eslint-disable*/
                console.log(item);
                this.$router.push({ name: 'asset-details', params: {id: item.id } })
            }
            this.deleting = false;
            this.powering = false;
            this.powering = false;
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
