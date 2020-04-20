<template>
    <v-card flat>
        <changePlanBar></changePlanBar>
        <v-card-title>{{formTitle}}</v-card-title>
        <v-container>
            <v-card flat>
                <v-layout>
                    <v-spacer></v-spacer>
                    <v-tooltip top>
                        <template v-slot:activator="{ on }">
                            <v-switch v-model="labelGen"
                                      label="Asset Label Mode"
                                      v-on="on"
                                      class="pa-3"></v-switch>
                        </template>

                        <span>Switch on to begin selecting labels for asset label generation</span>
                    </v-tooltip>
                </v-layout>
            </v-card>
        </v-container>

        <v-container>
            <v-card>
                <v-spacer></v-spacer>
                <v-data-table v-model="selectedAssets"
                              calculate-widths
                              :headers="filteredHeaders"
                              :items="assets"
                              :search="search"
                              class="pa-5"
                              @click:row="showDetails"
                              :server-items-length="totalItems"
                              :show-select="labelGen"
                              :options.sync="options"
                              show-expand
                              :expanded="getRowsToExpand"
                              :loading="loading">

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
                                                  label="Sites"
                                                  @change="initialize()"
                                                  placeholder="Select a site or all sites"
                                                  class="pt-8 pl-4">
                                        </v-select>
                                    </v-col>
                                </v-row>
                            </v-container>
                            <v-spacer></v-spacer>
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
                                                  @input="initialize()"
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
                                                  @input="initialize()"
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
                                                  @input="initialize()"
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
                                                  @input="initialize()"
                                                  placeholder="Start"
                                                  type="text"
                                                  label="Rack Range"
                                                  style="width:0">
                                    </v-text-field>
                                    <v-text-field v-model="endRackValue"
                                                  @input="initialize()"
                                                  type="text"
                                                  placeholder="End"
                                                  style="width:0">
                                    </v-text-field>
                                </v-row>
                            </v-col>
                            <v-spacer></v-spacer>
                        </v-row>


                    </template>

                    <!--Customize Select All/None Option-->
                    <template v-slot:header.data-table-select="{ on, props }">
                        <v-row>
                            <span style="white-space:nowrap">Select All</span>
                        </v-row>

                        <v-simple-checkbox color="primary"
                                           class="pb-4"
                                           v-bind="props"
                                           v-on="on"></v-simple-checkbox>
                    </template>

                    <!--Add Button to Data Table Footer-->
                    <template v-if="labelGen" v-slot:footer>
                        <v-row class="pa-10">
                            <v-spacer></v-spacer>
                            <v-tooltip bottom>
                                <template v-slot:activator="{ on }">
                                    <v-btn dark
                                           color="primary"
                                           v-on="on"
                                           @click="addLabels">
                                        Generate Labels
                                    </v-btn>
                                </template>

                                <span>Generate barcode labels for the current selection of assets</span>
                            </v-tooltip>
                            <v-spacer></v-spacer>
                        </v-row>
                    </template>

                    <!-- TODO: integrate Expandable Rows for Blades -->
                    <template v-slot:item.data-table-expand="{ item, isExpanded, expand }">
                        <v-btn icon v-if="isExpanded && item.mountType ==='chassis'"
                               @click="editing=true; expand(false)">
                            <v-icon>mdi-chevron-up</v-icon>
                        </v-btn>
                        <v-btn icon v-if="!isExpanded && item.mountType ==='chassis'"
                               @click="editing=true; expand(true)">
                            <v-icon>mdi-chevron-down</v-icon>
                        </v-btn>
                    </template>
                    <template v-slot:expanded-item="{ headers, item }">
                        <td v-if="showBlade(item)"
                            :colspan="headers.length">
                            <thead>
                                <th>Blades</th>
                                <th>Vendor</th>
                                <th>Model No.</th>
                                <th>Asset No.</th>
                                <th>Hostname</th>
                                <th>Datacenter</th>
                                <th>Slot</th>
                                <th>Owner</th>
                                <th>Power</th>
                                <th>Actions</th>
                            </thead>
                            <tbody>
                                <tr v-for="(blade, index) in item.blades"
                                    :key="index"
                                    @click="showDetails(blade)">
                                    <td>{{index+1}}</td>
                                    <td>{{item.blades[index].vendor}}</td>
                                    <td>{{item.blades[index].modelNumber}}</td>
                                    <td>{{item.blades[index].assetNumber}}</td>
                                    <td>{{item.blades[index].hostname}}</td>
                                    <td>{{item.blades[index].datacenter}}</td>
                                    <td>{{item.blades[index].chassisSlot}}</td>
                                    <td>{{item.blades[index].owner}}</td>
                                    <td>
                                        <!--Power-->
                                        <!--v-if="displayPower(item)" ??-->
                                        <v-row>
                                            <v-item-group dense
                                                          light
                                                          tile>
                                                <v-btn color="green lighten-1"
                                                       @click="turnOn(item.blades[index])"
                                                       x-small
                                                       width="30%"
                                                       min-width="30px"
                                                       depressed>
                                                    ON
                                                </v-btn>
                                                <v-btn color="red lighten-1"
                                                       @click="turnOff(item.blades[index])"
                                                       min-width="30px"
                                                       x-small
                                                       width="30%"
                                                       depressed>
                                                    OFF
                                                </v-btn>
                                            </v-item-group>
                                        </v-row>
                                    </td>
                                    <td>
                                        <!--Actions-->
                                        <v-row>
                                            <v-tooltip top>
                                                <template v-slot:activator="{ on }">
                                                    <v-btn icon v-on="on"
                                                           @click="editItem(item.blades[index])">
                                                        <v-icon medium
                                                                class="mr-2">
                                                            mdi-pencil
                                                        </v-icon>
                                                    </v-btn>
                                                </template>

                                                <span>Edit Asset</span>
                                            </v-tooltip>


                                            <v-tooltip v-if="assetType==='active'" top>
                                                <template v-slot:activator="{ on }">
                                                    <v-btn icon v-on="on"
                                                           @click="moveAsset(item.blades[index])">
                                                        <v-icon medium
                                                                class="mr-2">
                                                            mdi-server-minus
                                                        </v-icon>
                                                    </v-btn>
                                                </template>

                                                <span>Move To Offline Storage</span>
                                            </v-tooltip>

                                            <v-tooltip v-if="assetType==='offline'" top>
                                                <template v-slot:activator="{ on }">
                                                    <v-btn icon v-on="on"
                                                           @click="moveAsset(item.blades[index])">
                                                        <v-icon medium
                                                                class="mr-2">
                                                            mdi-server-plus
                                                        </v-icon>
                                                    </v-btn>
                                                </template>

                                                <span>Move To Datacenter</span>
                                            </v-tooltip>

                                            <v-tooltip top>
                                                <template v-slot:activator="{ on }">
                                                    <v-btn icon v-on="on"
                                                           @click="decommissionItem(item.blades[index])">
                                                        <v-icon medium
                                                                class="mr-2">
                                                            mdi-archive-arrow-down
                                                        </v-icon>
                                                    </v-btn>
                                                </template>

                                                <span>Decommission Asset</span>
                                            </v-tooltip>

                                            <v-tooltip top>
                                                <template v-if="!changePlanId()" v-slot:activator="{ on }">
                                                    <v-btn icon v-on="on"
                                                           @click="deleteItem(item.blades[index])">
                                                        <v-icon medium
                                                                class="mr-2">
                                                            mdi-delete
                                                        </v-icon>
                                                    </v-btn>
                                                </template>

                                                <span>Delete Asset</span>
                                            </v-tooltip>

                                        </v-row>
                                    </td>
                                </tr>
                            </tbody>
                        </td>
                        <td v-else :colspan="headers.length">This chassis has no blades</td>
                    </template>

                    <!-- Custom Location Column -->
                    <template v-slot:item.location="{ item }">
                        Rack {{ item.rack }}, {{ item.rackPosition }} U
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


                            <v-tooltip v-if="assetType==='active'" top>
                                <template v-slot:activator="{ on }">
                                    <v-btn icon v-on="on"
                                           @click="moveAsset(item)">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-server-minus
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Move To Offline Storage</span>
                            </v-tooltip>

                            <v-tooltip v-if="assetType==='offline'" top>
                                <template v-slot:activator="{ on }">
                                    <v-btn icon v-on="on"
                                           @click="moveAsset(item)">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-server-plus
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Move To Datacenter</span>
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
                                <template v-if="!changePlanId()" v-slot:activator="{ on }">
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
                        <v-row v-if="displayPower(item)">
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

            <v-snackbar v-model="updateSnackbar.show"
                        :bottom=true
                        class="black--text"
                        :color="updateSnackbar.color"
                        :timeout=5000>
                {{updateSnackbar.message}}
                <v-btn dark
                       class="black--text"
                       text
                       @click="updateSnackbar.show = false">
                    Close
                </v-btn>
            </v-snackbar>

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
        props: ['type'],
        data() {
            return {
                changePlanner: false,
                labelGen: false,
                selectedAssets: [],
                selectedDatacenter: 'All Sites',
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
                toActive: false,
                toOffline: false,
                refresh: 0,
                // Table data.
                headers: [
                    { text: 'Model Vendor', value: 'vendor' },
                    { text: 'Model No.', value: 'modelNumber', },
                    { text: 'Asset No.', value: 'assetNumber'},
                    { text: 'Hostname', value: 'hostname' },
                    { text: 'Site', value: 'datacenter' },
                    { text: 'Location', value: 'location' },
                    { text: 'Owner', value: 'owner' },
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
                    changePlanId: '',
                    isOffline: false,
                },
                editing: false,
                updateSnackbar: {
                    show: false,
                    message: '',
                    color: ''
                },
                assetType: this.type,
            }
        },
        beforeRouteUpdate(to, from, next) {
            /*eslint-disable*/
            this.assetType = to.params.type;
            this.$route.params.type = to.params.type;
            this.createPage();

            console.log(from);
            next()
        },
        computed: {
            permission() {
                return this.$store.getters.hasAssetPermission || this.$store.getters.isAdmin
            },
            powerPermission() {
                return this.$store.getters.hasPowerPermission || this.$store.getters.isAdmin
            },
            filteredHeaders() {
                var newHeaders = this.headers;

                if (!this.powerPermission || this.$store.getters.isChangePlan || this.assetType==='offline') {
                    newHeaders = newHeaders.filter(h => h.text !== "Power")
                }
                if (!this.permission) {
                    newHeaders = newHeaders.filter(h => h.text !== "Actions")
                }
                if (this.assetType === 'offline') {
                    newHeaders = newHeaders.filter(h => h.text !== "Location")
                }

                return newHeaders;
            },
            formTitle() {
                if (this.assetType === 'active') {
                    return 'Assets';
                } else {
                    return 'Assets in Offline Storage';
                }
            },
            getRowsToExpand() {
                var arr = [];
                var index = 0;
                this.assets.forEach(item => {
                    if (item.mountType === 'chassis') {
                        arr[index] = item;
                        index += 1;
                    }
                })
                return arr;
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
        async created() { 
            /* eslint-disable no-unused-vars, no-console */
            console.log(this.assetType)
            this.createPage();
        },
        methods: {
            createPage() {
                this.initializeDatacenters();
                this.initialize();
                //this.getAssetsFromApi();
                if (this.$store.getters.isChangePlan) {
                    this.modifyAssetsForChangePlan();
                }
            },
            showBlade(item) {
                if (typeof item.blades !== 'undefined' && item.blades.length > 0)
                    return true
                return false;
            },
            async getAssetsFromApi() {
                this.loading = true;
                const { sortBy, sortDesc, page, itemsPerPage } = this.options;

                this.fillQuery(sortBy, sortDesc, page, itemsPerPage);
                console.log("this is the sorting stuff")
                console.log(this.assetSearchQuery);

                this.assetSearchQuery.changePlanId = this.changePlanId();

                var info = await this.assetRepository.tablelist(this.assetSearchQuery);
   
                this.assets = info.data;
                return info;
            },
            async fillQuery(sortBy, sortDesc, page, itemsPerPage) {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);
                console.log(searchDatacenter);
                if (typeof searchDatacenter === 'undefined') {
                    this.assetSearchQuery.datacenter = '';
                } else {
                    this.assetSearchQuery.datacenter = searchDatacenter.id;
                }
                
                if (this.assetType === 'offline') {
                    this.assetSearchQuery.isOffline = true;
                } else {
                    this.assetSearchQuery.isOffline = false;
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
                await this.getAssetsFromApi()
                    .then(data => {
                        this.assets = data.data;
                        this.totalItems = data.totalCount;
                        this.loading = false;
                    });
            },
            async initializeDatacenters() {
                var datacenter;
                if (this.$store.getters.isChangePlan) {
                    this.selectedDatacenter = this.$store.getters.changePlan.datacenterDescription; //Limiting change plans to a datacenter
                    this.datacenters = [];
                    datacenter = {
                        description: this.selectedDatacenter,
                        name: this.$store.getters.changePlan.datacenterName,
                        id: this.$store.getters.changePlan.datacenterId,
                    }
                } else {
                    if (this.assetType === 'offline') {
                        this.datacenters = await this.datacenterRepository.listOffline();
                    } else {
                        this.datacenters = await this.datacenterRepository.list();
                    }
                    datacenter = {
                        description: "All Sites",
                        name: "All",
                    }
                }
                this.datacenters.push(datacenter);
                this.loading = false;
            },
            deleteItem(item) {
                this.editing = true;
                confirm('Are you sure you want to delete this asset?') && this.assetRepository.delete(item.id)
                    .then(async () => {
                        await this.initialize();
                    })
            },
            async decommissionItem(item) {
                this.editing = true;
                var graph = await networkNeighborhood.createGraph(item.id, this.changePlanId());
                var query = { Id: item.id, NetworkPortGraph: JSON.stringify(graph), Decommissioner: this.$store.state.username }
                query.changePlanId = this.changePlanId();
                /*eslint-disable*/
                console.log(query);
                confirm('Are you sure you want to decommission this asset? \nThis will remove the asset from the assets table, and instead add it to the decommissioned assets table.') && this.assetRepository.decommission(query)
                    .then(async () => {
                        await this.initialize();
                    })
            },
            // Is this unnecessary?
            async datacenterSearch() {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);
                this.assets = await this.assetRepository.list(searchDatacenter.id);
            },
            editItem(item) {
                this.editing = true;
                console.log(item);
                this.$router.push({ name: 'asset-edit', params: { id: item.id, type: this.assetType } })
            },
            addItem() {
                this.$router.push({ name: 'asset-new', params: { type: this.assetType } })
            },
            addLabels() {
                /*eslint-disable*/
                console.log(this.selectedAssets);
                this.$router.push({ name: 'asset-labels', params: { assets: this.selectedAssets } })
            },
            displayPower(item) {
                return item.hasNetworkManagedPower && (this.powerPermission || (item.ownerId == this.$store.getters.userId))
            },
            async turnOn(item) {
                console.log(item);
                this.editing = true;
                var powerState = {
                    // 0 is on
                    action: 0,
                };
                if (confirm('Are you sure you would like to turn on this asset?')) {
                    try {
                        await this.assetRepository.postPowerState(item.id, powerState);
                    } catch (e) {
                        console.log('ERROR')
                        alert('PDU Site Unavailable');
                    }
                    await this.initialize();
                }
            },
            async turnOff(item) {
                this.editing = true;
                var powerState = {
                    // 1 is off
                    action: 1,
                };
                if (confirm('Are you sure you would like to turn off this asset?')) {
                    try {
                        await this.assetRepository.postPowerState(item.id, powerState);
                    } catch (e) {
                        console.log('ERROR')
                        alert('PDU Site Unavailable');
                    }
                    await this.initialize();
                }
            },
            async cycle(item) {
                this.editing = true;
                var powerState = {
                    // 2 is cycle
                    action: 2,
                };
                if (confirm('Are you sure you would like to cycle this asset?')) {
                    try {
                        await this.assetRepository.postPowerState(item.id, powerState);
                    } catch (e) {
                        console.log('ERROR')
                        alert('PDU Site Unavailable');
                    }
                    await this.initialize();
                }
            },
            showInstructions() {
                this.instructionsDialog = true;
            },
            closeDetail() {
                this.instructionsDialog = false;
            },
            changePlanId() {
                if (this.$store.getters.isChangePlan)
                    return this.$store.getters.changePlan.id;
            },
            showDetails(item) {
                if (!this.editing) {
                    this.$router.push({ name: 'asset-details', params: { id: item.id, type: this.assetType } })
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
                console.log(this.selectedDatacenter);
                console.log("In a change plan!");
            },
            // TODO: ideally moving an asset offline will take an id (will ensure this works for blades as well)
            moveAsset(item) {
                this.editing = true;
                this.$router.push({ name: 'move-asset', params: {type: this.assetType, item: item}})
            },
        },
    }
</script>
