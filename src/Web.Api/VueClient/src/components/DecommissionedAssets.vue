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
                                    <v-col cols="5">
                                        <v-select v-model="selectedDatacenter"
                                                  :items="datacenters"
                                                  item-text="description"
                                                  item-value=""
                                                  :return-object="false"
                                                  label="Datacenter"
                                                  placeholder="Select a datacenter or all datacenters"
                                                  class="pt-8 pl-4"
                                                  range
                                                  @change="datacenterSearch()">
                                        </v-select>
                                    </v-col>
                                    <v-col class="mt-6" cols="4">
                                        <v-menu ref="menu"
                                                v-model="menu"
                                                :close-on-content-click="false"
                                                :return-value.sync="dates"
                                                transition="scale-transition"
                                                offset-y
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
                                    <v-autocomplete prepend-inner-icon="mdi-magnify"
                                                    :items="assets"
                                                    :search-input.sync="search"
                                                    cache-items
                                                    class="mt-3 pt-3"
                                                    flat
                                                    hide-no-data
                                                    hide-details
                                                    item-text="modelName"
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
                dates: ['2020-03-01', '2020-03-02'],
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
                    { text: 'Time Decommissioned', value: 'date' },
                    { text: 'Decomissioned By User', value: 'decommissioner' },
                    { text: 'Model Vendor', value: 'modelName' },
                    { text: 'Model Number', value: 'modelNumber', },
                    { text: 'Hostname', value: 'hostname' },
                    { text: 'Datacenter', value: 'datacenter' },
                    { text: 'Rack', value: 'rackAddress', filter: this.rackFilter },
                    { text: 'Rack U', value: 'data.RackPosition', },
                    { text: 'Owner Username', value: 'data.OwnerName' },
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
        },

        async created() {
            this.initialize();
            this.$store.dispatch('loadUsername');
        },

        methods: {
            async initialize() {
                this.assets = await this.assetRepository.getDecommissionedAssets();
                this.models = await this.modelRepository.list();
                this.users = await this.userRepository.list();
                this.datacenters = await this.datacenterRepository.list();

                // Turn data blob into fields to be read in table
                this.assets.forEach(e => {
                    var assetInfo = JSON.parse(e.data);
                    e.data = assetInfo;
                })

                /*eslint-disable*/
                console.log(this.assets);

                var datacenter = {
                    description: "All Datacenters",
                    name: "All",
                }
                this.datacenters.push(datacenter);

                this.loading = false;

            },
            async datacenterSearch() {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);
                this.assets = await this.assetRepository.list(searchDatacenter.id);
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
