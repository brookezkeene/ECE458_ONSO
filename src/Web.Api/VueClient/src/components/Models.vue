<template>
    <div>
        <v-card flat>
            <v-card-title>Models</v-card-title>
            <v-container>
                <v-card>
                    <v-data-table :headers="filteredHeaders"
                                  :items="models"
                                  :search="search"
                                  :options.sync="options"
                                  :server-items-length="totalItems"
                                  class="pa-10"
                                  :loading="loading"
                                  @click:row="showDetails">
                        <template v-slot:top v-slot:item.action="{ item }">

                            <v-toolbar flat color="white">
                                <v-row>
                                    <v-col class="pt-3 mt-4" cols="2">
                                        <v-label>Filter by ...</v-label>
                                    </v-col>
                                    <v-col cols="4">

                                        <v-text-field prepend-inner-icon="mdi-magnify"
                                                      :search-input.sync="vendorSearch"
                                                      v-model="vendorSearch"
                                                      @input="getDataFromApi()"
                                                      cache-items
                                                      flat
                                                      hide-no-data
                                                      hide-details
                                                      label="Search Vendor"
                                                      single-line
                                                      solo-inverted>

                                        </v-text-field>

                                    </v-col>
                                    <v-col cols="4">

                                        <v-text-field prepend-inner-icon="mdi-magnify"
                                                      :search-input.sync="numberSearch"
                                                      v-model="numberSearch"
                                                      @input="getDataFromApi()"
                                                      cache-items
                                                      flat
                                                      hide-no-data
                                                      hide-details
                                                      label="Search Number"
                                                      single-line
                                                      solo-inverted>

                                        </v-text-field>

                                    </v-col>
                                </v-row>

                                <v-btn v-if="permission" color="primary" dark class="mb-2" @click="addItem">Add Model</v-btn>

                            </v-toolbar>

                            <v-row class="pl-4">
                                <v-col cols="11">
                                    <v-row>
                                        <!-- Custom filters; sorts between height ranges -->
                                        <v-col cols="3">
                                            <v-row>
                                                <v-col cols="6">
                                                    <v-text-field v-model="startHeightValue"
                                                                  placeholder="from"
                                                                  type="number"
                                                                  label="Height"
                                                                  @input="getDataFromApi()">
                                                    </v-text-field>
                                                </v-col>
                                                <v-col cols="6">
                                                    <v-text-field v-model="endHeightValue"
                                                                  type="number"
                                                                  placeholder="to"
                                                                  @input="getDataFromApi()">
                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <v-col cols="3">
                                            <v-row>
                                                <v-col cols="6">
                                                    <v-text-field v-model="startNetworkValue"
                                                                  placeholder="from"
                                                                  type="number"
                                                                  label="Network Ports"
                                                                  @input="getDataFromApi()">
                                                    </v-text-field>
                                                </v-col>
                                                <v-col cols="6">
                                                    <v-text-field v-model="endNetworkValue"
                                                                  type="number"
                                                                  placeholder="to"
                                                                  @input="getDataFromApi()">
                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <v-col cols="3">
                                            <v-row>
                                                <v-col cols="6">
                                                    <v-text-field v-model="startPowerValue"
                                                                  placeholder="from"
                                                                  type="number"
                                                                  label="Power Ports"
                                                                  @input="getDataFromApi()">
                                                    </v-text-field>
                                                </v-col>
                                                <v-col cols="6">
                                                    <v-text-field v-model="endPowerValue"
                                                                  type="number"
                                                                  placeholder="to"
                                                                  @input="getDataFromApi()">
                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                        <v-col cols="3">
                                            <v-row>
                                                <v-col cols="6">
                                                    <v-text-field v-model="startMemoryValue"
                                                                  placeholder="from"
                                                                  type="number"
                                                                  label="Memory"
                                                                  @input="getDataFromApi()">
                                                    </v-text-field>
                                                </v-col>
                                                <v-col cols="6">
                                                    <v-text-field v-model="endMemoryValue"
                                                                  type="number"
                                                                  placeholder="to"
                                                                  @input="getDataFromApi()">
                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>

                        </template>

                        <!-- Mount Type -->
                        <template v-slot:item.type="{ item }">
                            {{typeMap[item.mountType]}}
                        </template>

                        <!-- Blade Omitted Fields -->
                        <template v-slot:item.mHeight="{ item }">
                            <div v-if="item.mountType != 'blade'">
                                {{item.height}}
                            </div>
                            <div v-else>
                                N/A
                            </div>
                        </template>
                        <template v-slot:item.netPorts="{ item }">
                            <div v-if="item.mountType != 'blade'">
                                {{item.ethernetPorts}}
                            </div>
                            <div v-else>
                                N/A
                            </div>
                        </template>
                        <template v-slot:item.powPorts="{ item }">
                            <div v-if="item.mountType != 'blade'">
                                {{item.powerPorts}}
                            </div>
                            <div v-else>
                                N/A
                            </div>
                        </template>

                        <template v-slot:item.coloricon="{ item }">
                            <v-icon class="mr-2"
                                    :color=item.displayColor>
                                mdi-circle
                            </v-icon>
                        </template>

                        <template v-if="permission" v-slot:item.action="{ item }">
                            <v-row>
                                <v-icon medium
                                        class="mr-2"
                                        @click="editItem(item)">mdi-pencil</v-icon>
                                <v-icon medium
                                        class="mr-2"
                                        @click="deleteItem(item)">mdi-delete</v-icon>
                            </v-row>
                        </template>
                        <template v-slot:no-data>
                            <v-btn color="primary" @click="initialize">Refresh</v-btn>
                        </template>
                    </v-data-table>
                </v-card>
            </v-container>
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
        </v-card>
    </div>
</template>


<script>
    export default {
        components: {
        },
        inject: ['modelRepository'],
        data() {
            return {
                options: {},
                totalItems: 0,
                updateSnackbar: {
                    show: false,
                    message: '',
                    color: ''
                },
                // Filter values.
                startHeightValue: '',
                endHeightValue: '',
                startMemoryValue: '',
                endMemoryValue: '',
                startNetworkValue: '',
                endNetworkValue: '',
                startPowerValue: '',
                endPowerValue: '',
                vendorSearch: '',
                numberSearch: '',

                dialog: false,
                detailsDialog: false,
                loading: true,
                search: '',

                headers: [
                    {
                        text: 'Vendor',
                        align: 'left',
                        value: 'vendor'
                    },
                    { text: 'Model Number', value: 'modelNumber' },
                    { text: 'Mount Type', value: 'type' }, 
                    { text: 'Height', value: 'mHeight' },
                    { text: 'Display Color', value: 'coloricon', sortable: false },
                    { text: 'Network Ports', value: 'netPorts' }, 
                    { text: 'Power Ports', value: 'powPorts' },
                    { text: 'CPU', value: 'cpu' },
                    { text: 'Memory', value: 'memory' },
                    { text: 'Storage', value: 'storage' },
                    { text: 'Actions', value: 'action', sortable: false },

                ],
                models: [],

                typeMap: {
                    "normal" : "Normal", 
                    "chassis" : "Blade Chassis", 
                    "blade" : "Blade"
                },

                editedIndex: -1,
                editedItem: {
                    vendor: '',
                    modelNumber: '',
                    height: 0,
                    displayColor: '',
                    ethernetPorts: 0, // TODO: change to networkPorts
                    powerPorts: 0,
                    cpu: '',
                    memory: 0,
                    storage: '',
                    comment: ''
                },
                defaultItem: {
                    vendor: '',
                    modelNumber: '',
                    height: 0,
                    displayColor: '',
                    ethernetPorts: 0, // TODO: change to networkPorts
                    powerPorts: 0,
                    cpu: '',
                    memory: 0,
                    storage: '',
                    comment: ''
                },
                deleting: false,
                editing: false,
                searchQuery: {
                    vendor: '',
                    number: '',
                    heightStart: 0,
                    heightEnd: 0,
                    networkRangeStart: 0,
                    networkRangeEnd: 0,
                    powerRangeStart: 0,
                    powerRangeEnd: 0,
                    memoryRangeStart: 0,
                    memoryRangeEnd: 0,
                    page: 0,
                    pageSize: 0,
                    isDesc: '',
                    sortBy: '',
                },

            }
        },
        computed: {
            permission() {
                return this.$store.getters.hasModelPermission || this.$store.getters.isAdmin
            },
            formTitle() {
                return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
            },
            filteredHeaders() {
                return (this.permission) ? this.headers : this.headers.filter(h => h.text !== "Actions")
            },
        },
        watch: {
            dialog(val) {
                val || this.close()
            },
            detailsDialog(val) {
                val || this.closeDetail()
            },
            options: {
                handler() {
                    this.getDataFromApi()
                        .then(data => {
                            this.models = data.data;
                            this.totalItems = data.totalCount;
                            this.loading = false;
                        })
                },
                deep: true
            },
        },

        mounted() {
            this.getDataFromApi()
                .then(data => {
                    this.models = data.data;
                    this.totalItems = data.totalCount;
                    this.loading = false;
                })
        },
        methods: {
            async getDataFromApi() {
                this.loading = true;
                const { sortBy, sortDesc, page, itemsPerPage } = this.options;
                this.fillQuery(sortBy, sortDesc, page, itemsPerPage);

                /* eslint-disable no-unused-vars, no-console */
                console.log(sortBy)
                console.log(sortDesc)
                console.log("this is the sorting stuff")
                console.log(this.searchQuery);

                var info = await this.modelRepository.tablelist(this.searchQuery);
                this.models = info.data;
                this.loading = false;
                return info;
            },
            async initialize() {
                this.models = await this.modelRepository.tablelist(this.searchQuery);
                this.loading = false;
                /* eslint-disable no-unused-vars, no-console */
                console.log(this.permission)
            },
            editItem(item) {
                this.editing = true
                this.$router.push({ name: 'model-edit', params: { id: item.id } })
            },
            addItem() {
                this.$router.push({ name: 'model-create' })
            },
            async deleteItem(item) {
                this.deleting = true;
                var deleteModel = await this.modelRepository.find(item.id)

                if (deleteModel.assets != null && deleteModel.assets.length != 0) {
                    this.deleting = false;
                    this.updateSnackbar.show = true;
                    this.updateSnackbar.color = 'red lighten-4';
                    this.updateSnackbar.message = 'Assets of this model exist, cannot delete';
                    return;
                }
                confirm('Are you sure you want to delete this item?') && this.modelRepository.delete(item)
                    .then(async () => {
                        await this.initialize();
                        this.getDataFromApi()
                        .then(data => {
                            this.models = data.data;
                            this.totalItems = data.totalCount;
                            this.loading = false;
                        })

                    })
            },
            showDetails(item) {
                if (this.editedIndex === -1 && !this.deleting && !this.editing) {
                    this.$router.push({ name: 'model-details', params: { id: item.id } })
                }
                this.deleting = false;
            },
            fillQuery(sortBy, sortDesc, page, itemsPerPage) {
                this.searchQuery.vendor = this.vendorSearch;
                this.searchQuery.number = this.numberSearch;
                this.searchQuery.heightStart = this.parseToInt(this.startHeightValue);
                this.searchQuery.heightEnd = this.parseToInt(this.endHeightValue);
                this.searchQuery.memoryRangeStart = this.parseToInt(this.startMemoryValue);
                this.searchQuery.memoryRangeEnd = this.parseToInt(this.endMemoryValue);
                this.searchQuery.networkRangeStart = this.parseToInt(this.startNetworkValue);
                this.searchQuery.networkRangeEnd = this.parseToInt(this.endNetworkValue);
                this.searchQuery.powerRangeStart = this.parseToInt(this.startPowerValue);
                this.searchQuery.powerRangeEnd = this.parseToInt(this.endPowerValue);
                this.searchQuery.page = page;
                this.searchQuery.pageSize = itemsPerPage;
                this.searchQuery.sortBy = this.parseSort(sortBy);
                this.searchQuery.isDesc = this.parseSort(sortDesc);
            },
            parseToInt(value) {
                if (value == '') {
                    return 0;
                }
                return parseInt(value);
            },
            parseSort(value) {
                if (value.length !== 0) {
                    return value[0];
                }
                return '';
            },

        },
    }
</script>
