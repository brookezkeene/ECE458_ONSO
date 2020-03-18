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
                                  multi-sort
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
                                                      @input="getModels()"
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
                                                      @input="getModels()"
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
                                                                  label="Height">
                                                    </v-text-field>
                                                </v-col>
                                                <v-col cols="6">
                                                    <v-text-field v-model="endHeightValue"
                                                                  type="number"
                                                                  placeholder="to">
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
                                                                  label="Network Ports">
                                                    </v-text-field>
                                                </v-col>
                                                <v-col cols="6">
                                                    <v-text-field v-model="endNetworkValue"
                                                                  type="number"
                                                                  placeholder="to">
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
                                                                  label="Power Ports">
                                                    </v-text-field>
                                                </v-col>
                                                <v-col cols="6">
                                                    <v-text-field v-model="endPowerValue"
                                                                  type="number"
                                                                  placeholder="to">
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
                                                                  label="Memory">
                                                    </v-text-field>
                                                </v-col>
                                                <v-col cols="6">
                                                    <v-text-field v-model="endMemoryValue"
                                                                  type="number"
                                                                  placeholder="to">
                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </v-col>
                                    </v-row>
                                </v-col>
                            </v-row>

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
                            <v-btn color="primary" @click="initialize">Reset</v-btn>
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
                    { text: 'Model Number', value: 'modelNumber', },
                    { text: 'Height', value: 'height', filter: this.heightFilter },
                    { text: 'Display Color', value: 'coloricon', sortable: false },
                    { text: 'Network Ports', value: 'ethernetPorts', filter: this.networkFilter }, // TODO: change value to networkPorts!
                    { text: 'Power Ports', value: 'powerPorts', filter: this.powerFilter },
                    { text: 'CPU', value: 'cpu' },
                    { text: 'Memory', value: 'memory', filter: this.memoryFilter },
                    { text: 'Storage', value: 'storage' },
                    { text: 'Actions', value: 'action', sortable: false },

                ],
                models: [],
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
                query: {
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


        methods: {
            getDataFromApi() {
                this.loading = true;
                const { page, itemsPerPage } = this.options;
                return this.modelRepository.tablelist(page, itemsPerPage, this.search);
            },
            async getModels() {
                this.loading = true;
                const { page, itemsPerPage } = this.options;
                //fill the query with all the values in the filters
                this.fillQuery();
                /* eslint-disable no-unused-vars, no-console */
                console.log(this.query)
                var info = await this.modelRepository.tablelist(page, itemsPerPage, this.vendorSearch);
                this.models = info.data;
            },
            async initialize() {
                this.models = await this.modelRepository.list();
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
                    })
            },
            showDetails(item) {
                if (this.editedIndex === -1 && !this.deleting && !this.editing) {
                    this.$router.push({ name: 'model-details', params: {id: item.id } })
                }
                this.deleting = false;
            },
            fillQuery() {
                this.query.vendor = this.vendorSearch;
                this.query.number = this.numberSearch;
                this.query.heightStart = 0//this.startHeightValue;
                this.query.heightEnd = 0//this.endHeightValue;
                this.query.memoryRangeStart = 0//this.startMemoryValue;
                this.query.memoryRangeEnd = 0//this.endMemoryValue;
                this.query.networkRangeStart = 0//this.startNetworkValue;
                this.query.networkRangeEnd = 0//this.endNetworkValue;
                this.query.powerRangeStart = 0//this.startPowerValue;
                this.query.powerRangeEnd = 0//this.endPowerValue;
            },
            /**
             * Filter code below; TODO: refactor this
             * end/start values are the filter inputs
             * @param value is the value of the table entry
             */
            heightFilter(value) {
                // If this filter has no value we just skip the entire filter.
                if (!this.startHeightValue && !this.endHeightValue) {
                    return true;
                    // If only one filter has a value, leans entirely on that one filter
                } else if (!this.endHeightValue) {
                    return (value) >= parseInt(this.startHeightValue);
                } else if (!this.startHeightValue) {
                    return (value) <= parseInt(this.endHeightValue);
                }

                // Check if the current loop value (The Height value)
                // is between the Height values inputted
                return (value) >= parseInt(this.startHeightValue)
                    && (value) <= parseInt(this.endHeightValue);
            },
            memoryFilter(value) {
                if (!this.startMemoryValue && !this.endMemoryValue) {
                    return true;
                } else if (!this.endMemoryValue) {
                    return (value) >= parseInt(this.startMemoryValue);
                } else if (!this.startMemoryValue) {
                    return (value) <= parseInt(this.endMemoryValue);
                }

                return (value) >= parseInt(this.startMemoryValue)
                    && (value) <= parseInt(this.endMemoryValue);
            },
            networkFilter(value) {
                if (!this.startNetworkValue && !this.endNetworkValue) {
                    return true;
                } else if (!this.endNetworkValue) {
                    return (value) >= parseInt(this.startNetworkValue);
                } else if (!this.startNetworkValue) {
                    return (value) <= parseInt(this.endNetworkValue);
                }

                return (value) >= parseInt(this.startNetworkValue)
                    && (value) <= parseInt(this.endNetworkValue);
            },
            powerFilter(value) {
                if (!this.startPowerValue && !this.endPowerValue) {
                    return true;
                } else if (!this.endPowerValue) {
                    return (value) >= parseInt(this.startPowerValue);
                } else if (!this.startPowerValue) {
                    return (value) <= parseInt(this.endPowerValue);
                }

                return (value) >= parseInt(this.startPowerValue)
                    && (value) <= parseInt(this.endPowerValue);
            },


        },
    }
</script>
