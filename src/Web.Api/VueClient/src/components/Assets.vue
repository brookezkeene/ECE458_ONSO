<template>
    <v-card flat>
        <changePlanBar></changePlanBar>
        <v-card-title>Assets</v-card-title>
        <v-container>
            <v-card>
                <v-spacer></v-spacer>
                <v-data-table :headers="filteredHeaders"
                              :items="assets"
                              :search="search"
                              class="pa-5"
                              @click:row="showDetails"
                              :server-items-length="totalItems"
                              :options.sync="options"
                              :key="selectedDatacenter">

                    <template v-slot:top v-slot:item.action="{ item }">
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
                                                  class="pt-8 pl-4">
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
                            <v-col cols="4">
                                <v-row class="pl-10 pt-1">
                                    <v-text-field prepend-inner-icon="mdi-magnify"
                                                  :search-input.sync="vendorSearch"
                                                  v-model="vendorSearch"
                                                  @input="getAssetsFromApi()"
                                                  cache-items
                                                  flat
                                                  hide-no-data
                                                  hide-details
                                                  label="Search Vendor"
                                                  single-line
                                                  solo-inverted>

                                    </v-text-field>

                                </v-row>
                            </v-col>
                            <v-col cols="4">
                                <v-row class="pl-5 pt-1">
                                    <v-text-field prepend-inner-icon="mdi-magnify"
                                                  :search-input.sync="numberSearch"
                                                  v-model="numberSearch"
                                                  @input="getAssetsFromApi()"
                                                  cache-items
                                                  flat
                                                  hide-no-data
                                                  hide-details
                                                  label="Search Model Number"
                                                  single-line
                                                  solo-inverted>

                                    </v-text-field>

                                </v-row>
                            </v-col>

                            <v-col cols="4">
                                <v-row class="pl-5 pt-1 pr-10">
                                    <v-text-field prepend-inner-icon="mdi-magnify"
                                                  :search-input.sync="hostnameSearch"
                                                  v-model="hostnameSearch"
                                                  @input="getAssetsFromApi()"
                                                  cache-items
                                                  flat
                                                  hide-no-data
                                                  hide-details
                                                  label="Search Hostname"
                                                  single-line
                                                  solo-inverted>

                                    </v-text-field>

                                </v-row>
                            </v-col>

                            <!-- Custom filters; sorts between rack ranges -->
                        </v-row>
                        <v-row>
                            <v-col cols="6">
                                <v-row class="pl-10 pt-1">
                                    <v-text-field v-model="startRackValue"
                                                  @input="getAssetsFromApi()"
                                                  placeholder="Start"
                                                  type="text"
                                                  label="Rack Range"
                                                  style="width:0">
                                    </v-text-field>
                                    <v-text-field v-model="endRackValue"
                                                  @input="getAssetsFromApi()"
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
                        <v-row v-if="item.hasNetworkManagedPower && powerPermission">
                            <v-item-group dense
                                          light
                                          tile>
                                <v-btn color="green lighten-1"
                                       @click="turnOn(item)"
                                       x-small
                                       width="30%"
                                       min-width="30px"
                                       depressed>
                                    ON
                                </v-btn>
                                <v-btn color="red lighten-1"
                                       @click="turnOff(item)"
                                       min-width="30px"
                                       x-small
                                       width="30%"
                                       depressed>
                                    OFF
                                </v-btn>
                                <v-btn color="grey lighten-2"
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

    import networkNeighborhood from '@/networkNeighborhood';
    import changePlanBar from '@/components/ChangePlanStatusBar';

    export default {

        components: {
            changePlanBar,
        },
        inject: ['assetRepository', 'datacenterRepository'],
        data() {
            return {
                changePlanner: false,
                selectedDatacenter: 'All Datacenters',
                // Filter values.
                hostnameSearch: '',
                vendorSearch: '',
                numberSearch: '',
                startRackValue: '',
                endRackValue: '',

                datacenters: [],
                instructionsDialog: false,
                loading: true,
                search: '',
                options: {},
                totalItems: 0,


                // Table data.
                headers: [
                    { text: 'Model Vendor', value: 'vendor' },
                    { text: 'Model Number', value: 'modelNumber', },
                    { text: 'Hostname', value: 'hostname' },
                    { text: 'Datacenter', value: 'datacenter' },
                    { text: 'Rack', value: 'rack' },
                    { text: 'Rack U', value: 'rackPosition', },
                    { text: 'Owner Username', value: 'owner' },
                    { text: 'Power', value: 'power', sortable: false },
                    { text: 'Actions', value: 'action', sortable: false },
                ],
                assets: [],
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
                assetSearchQuery: {
                    datacenter: '',
                    hostname: '',
                    rackStart: '',
                    rackEnd: '',
                    vendor: '',
                    modelNumber: '',
                    page: 0,
                    pageSize: 0,
                    isDesc: '',
                    sortBy: '',
                },
                editing: false,
            }
        },
        computed: {
            permission() {
                return this.$store.getters.hasAssetPermission || this.$store.getters.isAdmin
            },
            powerPermission() {
                return this.$store.getters.hasPowerPermission || this.$store.getters.isAdmin
            },
            filteredHeaders() {
                return (this.permission) ? this.headers : this.headers.filter(h => h.text !== "Actions" && h.text !== "Power")
            },
        },
        watch: {
            instructionsDialog(val) {
                val || this.closeDetail()
            },
            options: {
                handler() {
                    this.getAssetsFromApi()
                        .then(data => {
                            this.assets = data.data;
                            this.totalItems = data.totalCount;
                            this.loading = false;
                        })
                },
                deep: true
            },
        },
        mounted() {
            this.getAssetsFromApi()
                .then(data => {
                    this.assets = data.data;
                    this.totalItems = data.totalCount;
                    this.loading = false;
                })
        },

        async created() {
            this.initializeDatacenters();
            this.initialize();
            if (this.$store.getters.isChangePlan) {
                this.modifyAssetsForChangePlan();
            }

        },

        methods: {
            async getAssetsFromApi() {
                this.loading = true;
                const { sortBy, sortDesc, page, itemsPerPage } = this.options;
                
                this.fillQuery(sortBy, sortDesc, page, itemsPerPage);
                /* eslint-disable no-unused-vars, no-console */
                console.log("this is the sorting stuff")
                console.log(this.assetSearchQuery);

                var info = await this.assetRepository.tablelist(this.assetSearchQuery);
                this.assets = info.data;
                return info;
            },
            async fillQuery(sortBy, sortDesc, page, itemsPerPage) {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);
                if (typeof searchDatacenter === 'undefined') {
                    this.assetSearchQuery.datacenter = '';
                } else {
                    this.assetSearchQuery.datacenter = searchDatacenter.id;
                }
                this.assetSearchQuery.vendor = this.vendorSearch;
                this.assetSearchQuery.modelNumber = this.numberSearch;
                this.assetSearchQuery.hostname = this.hostnameSearch;
                this.assetSearchQuery.rackStart = this.startRackValue;
                this.assetSearchQuery.rackEnd = this.endRackValue;
                this.assetSearchQuery.page = page;
                this.assetSearchQuery.pageSize = itemsPerPage;
                this.assetSearchQuery.sortBy = this.parseSort(sortBy);
                this.assetSearchQuery.isDesc = this.parseSort(sortDesc);
            },
            parseSort(value) {
                if (typeof value === 'undefined') {
                    return '';
                }
                else if (value.length !== 0) {
                    return value[0];
                }
                return '';
            },
            async initialize() {
                await this.getAssetsFromApi();
                this.loading = false;
            },
            async initializeDatacenters() {
                this.datacenters = await this.datacenterRepository.list();
                var datacenter = {
                    description: "All Datacenters",
                    name: "All",
                }
                this.datacenters.push(datacenter);
                this.loading = false;
            },
            deleteItem(item) {
                this.editing = true;
                confirm('Are you sure you want to delete this asset?') && this.assetRepository.delete(item)
                    .then(async () => {
                        await this.initialize();
                    })
            },
            async decommissionItem(item) {
                this.editing = true;
                var graph = await networkNeighborhood.createGraph(item.id);
                var query = { Id: item.id, NetworkPortGraph: JSON.stringify(graph), Decommissioner: this.$store.state.username }
                /*eslint-disable*/
                console.log(query);
                confirm('Are you sure you want to decommission this asset? \nThis will remove the asset from the assets table, and instead add it to the decommissioned assets table.') && this.assetRepository.decommission(query)
                    .then(async () => {
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
                this.editing = true;
                var powerState = {
                    // 0 is on
                    action: 0,
                };
                confirm('Are you sure you would like to turn on this asset?') && this.assetRepository.postPowerState(item.id, powerState)
                    .then(async () => {
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
                        await this.initialize();
                    })
            },
            cycle(item) {
                this.editing = true;
                var powerState = {
                    // 2 is cycle
                    action: 2,
                };
                confirm('Are you sure you would like to cycle this asset?') && this.assetRepository.postPowerState(item.id, powerState)
                    .then(async () => {
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
                    this.$router.push({ name: 'asset-details', params: { id: item.id } })
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
            async modifyAssetsForChangePlan() {
                console.log("In a change plan!");
            },
        },
    }
</script>
