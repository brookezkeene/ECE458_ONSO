﻿
<template>
    <v-card flat>
        <v-card-title>Decommissioned Assets</v-card-title>
        <v-container>
            <v-card>
                <v-spacer></v-spacer>
                <v-data-table :headers="filteredHeaders"
                              :items="assets"
                              :search="search"
                              class="pa-5"
                              @click:row="showDetails"
                              :server-items-length="totalItems"
                              :options.sync="options">

                    <template v-slot:top>
                        <v-toolbar flat>

                            <!-- ADDED AUTOCOMPLETE TO THE MODEL SEARCH -->
                            <v-container fluid align="left">
                                <v-row>
                                    <v-col class="pt-6 mt-6" cols="2">
                                        <v-label>Filter by ... </v-label>
                                    </v-col>
                                    <v-col cols="5">
                                        
                                        <v-text-field prepend-inner-icon="mdi-magnify"
                                                      :search-input.sync="datacenterValue"
                                                      v-model="datacenterValue"
                                                      cache-items
                                                      class="mt-3 pt-3"
                                                      flat
                                                      hide-no-data
                                                      hide-details
                                                      @input="getAssetsFromApi()"
                                                      label="Search by Datacenter name"
                                                      single-line
                                                      solo-inverted></v-text-field>
                                    </v-col>
                                    <v-col class="mt-6" cols="4">
                                        <v-menu ref="menu"
                                                v-model="menu"
                                                :close-on-content-click="false"
                                                :return-value.sync="dates"
                                                transition="scale-transition"
                                                offset-y
                                                @input="getAssetsFromApi()"
                                                @change="getAssetsFromApi()"
                                                min-width="290px">
                                            <template v-slot:activator="{ on }">
                                                <v-row>
                                                    <v-col>
                                                        <v-text-field v-model="dates[0]"
                                                                      label="Dates"
                                                                      prepend-icon="mdi-calendar"
                                                                      readonly
                                                                      v-on="on"></v-text-field>
                                                    </v-col>
                                                    <v-col>
                                                        <v-text-field v-model="dates[1]"
                                                                      readonly
                                                                      v-on="on"></v-text-field>
                                                    </v-col>
                                                    
                                                </v-row>

                                            </template>
                                            <v-date-picker v-model="dates" no-title range color="primary">
                                                <v-spacer></v-spacer>
                                                <v-btn text color="primary" @click="menu = false">Cancel</v-btn>
                                                <v-btn text color="primary" @click="$refs.menu.save(dates)">OK</v-btn>
                                            </v-date-picker>
                                        </v-menu>
                                    </v-col>
                                    
                                </v-row>

                            </v-container>

                            <v-spacer></v-spacer>

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
                                    <v-text-field prepend-inner-icon="mdi-magnify"
                                                  :search-input.sync="search"
                                                  v-model="search"
                                                  cache-items
                                                  class="mt-3 pt-3"
                                                  flat
                                                  hide-no-data
                                                  hide-details
                                                  @input="getAssetsFromApi()"
                                                  label="Search by keyword on model and hostname"
                                                  single-line
                                                  solo-inverted></v-text-field>

                                </v-row>
                            </v-col>
                            <v-col cols="6">
                                <v-row class="pl-10 pt-1">
                                    <v-text-field prepend-inner-icon="mdi-magnify"
                                                  v-model="decommissioner"
                                                  cache-items
                                                  class="mt-3 pt-3"
                                                  flat
                                                  hide-no-data
                                                  hide-details
                                                  label="Search for decommissioner"
                                                  @input="getAssetsFromApi()"
                                                  single-line
                                                  solo-inverted></v-text-field>

                                </v-row>
                            </v-col>
                        </v-row>
                        <v-row>
                        <v-spacer></v-spacer>
                        <!-- Custom filters; sorts between rack ranges -->
                        <v-col cols="4">
                            <v-row class="mt-4 pt-2">
                                <v-text-field v-model="startRackValue"
                                              placeholder="Start"
                                              type="text"
                                              label="Rack Range"
                                              @input="getAssetsFromApi()"
                                              style="width:0">
                                </v-text-field>

                                <v-text-field v-model="endRackValue"
                                              type="text"
                                              placeholder="End"
                                              @input="getAssetsFromApi()"
                                              style="width:0">
                                </v-text-field>
                            </v-row>
                        </v-col>
                        <v-spacer></v-spacer>
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
        inject: ['assetRepository', 'modelRepository', 'userRepository', 'datacenterRepository'],
        data() {
            return {
                menu: false,
                dates: ['', ''],
                selectedDatacenter: 'All Datacenters',
                datacenterValue: '',
                // Filter values.
                startRackValue: '',
                endRackValue: '',
                decommissioner:'',
                datacenters: [],
                instructionsDialog: false,
                loading: true,
                search: '',
                options: {},
                // Table data.
                headers: [
                    { text: 'Time Decommissioned', value: 'dateDecommissioned' },
                    { text: 'Decomissioned By User', value: 'decommissioner' },
                    { text: 'Asset Number', value: 'data.AssetNumber'},
                    { text: 'Model Vendor', value: 'modelName' },
                    { text: 'Model Number', value: 'modelNumber', },
                    { text: 'Hostname', value: 'hostname' },
                    { text: 'Datacenter', value: 'datacenter' },
                    { text: 'Rack', value: 'rackAddress', filter: this.rackFilter },
                    { text: 'Rack U', value: 'rackPosition', },
                    { text: 'Owner Username', value: 'ownerName' },
                ],
                totalItems: 0,
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
                assetSearchQuery: {
                    datacenterName: '',
                    generalSearch: '',
                    rackStart: '',
                    rackEnd: '',
                    decommissioner: '',
                    dateStart: '',
                    dateEnd: '',
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
            filteredHeaders() {
                return (this.permission) ? this.headers : this.headers.filter(h => h)
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
                            // Turn data blob into fields to be read in table
                            this.assets.forEach(e => {
                                var assetInfo = JSON.parse(e.data);
                                e.data = assetInfo;
                            })
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
                    // Turn data blob into fields to be read in table
                    this.assets.forEach(e => {
                        var assetInfo = JSON.parse(e.data);
                        e.data = assetInfo;
                    })
                    /*eslint-disable*/
                    console.log(this.assets);
                    this.totalItems = data.totalCount;
                    this.loading = false;
                })
        },


        async created() {
            this.$store.dispatch('loadUsername');
        },

        methods: {

            async getAssetsFromApi() {
                this.loading = true;
                const { sortBy, sortDesc, page, itemsPerPage } = this.options;
                
                this.fillQuery(sortBy, sortDesc, page, itemsPerPage);
                /* eslint-disable no-unused-vars, no-console */
                console.log("this is the sorting stuff")
                console.log(this.assetSearchQuery);

                var info = await this.assetRepository.tablelistDecommissionedAssets(this.assetSearchQuery);
                if ((page - 1) * itemsPerPage > info.totalCount) {
                    this.fillQuery(sortBy, sortDesc, 1, itemsPerPage);
                    info = await this.assetRepository.tablelistDecommissionedAssets(this.assetSearchQuery);
                }
                this.assets = info.data;
                return info;
            },
            fillQuery(sortBy, sortDesc, page, itemsPerPage) {
               
                this.assetSearchQuery.datacenterName = this.datacenterValue;
                this.assetSearchQuery.generalSearch = this.search;
                this.assetSearchQuery.decommissioner = this.decommissioner;
                this.assetSearchQuery.dateStart = this.dates[0];
                this.assetSearchQuery.dateEnd = this.dates[1];
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
            async datacenterSearch() {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);
                this.fillQuery();
                console.log(this.assetSearchQuery);
                console.log("this is the search query");
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
                    this.$router.push({ name: 'decommissioned-asset-details', params: { id: item.id } })
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
