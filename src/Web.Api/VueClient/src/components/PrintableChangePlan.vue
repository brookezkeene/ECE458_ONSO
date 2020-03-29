
<template>
    <v-card flat>
        <v-card-title>
            <span>Work Order Summary:</span>
            <span v-if="$store.getters.changePlan.name">
                {{$store.getters.changePlan.name}} in datacenter {{$store.getters.changePlan.datacenterName}}
            </span>
        </v-card-title>
        <div id='print'>
            <v-container>
                <v-card flat>
                    <v-spacer></v-spacer>
                    <v-data-table :headers="headers"
                                  :items="changePlanItems"
                                  class="pa-5"
                                  multi-sort
                                  show-expand
                                  :expanded="getRowsToExpand"
                                  disable-pagination
                                  hide-default-footer>

                        <template v-slot:item.data-table-expand="{ item, isExpanded, expand }">
                            <v-btn icon v-if="isExpanded"><v-icon>mdi-chevron-up</v-icon></v-btn>
                        </template>

                        <template v-slot:expanded-item="{ headers, item }">
                            <td v-if="item.executionType=='update'"></td>
                            <td v-if="item.executionType=='update'">{{item.previousData.Vendor}}</td>
                            <td v-if="item.executionType=='update'">{{item.previousData.ModelNumber}}</td>
                            <td v-if="item.executionType=='update'">{{item.previousData.AssetNumber}}</td>
                            <td v-if="item.executionType=='update'">{{item.previousData.Hostname}}</td>
                            <td v-if="item.executionType=='update'">{{item.previousData.Datacenter}}</td>
                            <td v-if="item.executionType=='update'">{{item.previousData.Rack}}</td>
                            <td v-if="item.executionType=='update'">{{item.previousData.RackPosition}}</td>
                            <td v-if="item.executionType=='update'">{{item.previousData.Owner}}</td>

                        </template>

                        <template v-slot:item.executed="{ item }">
                            <div v-if="item.executedDate">
                                <v-icon color="primary">
                                    mdi-check-circle-outline
                                </v-icon>
                            </div>
                            <div v-else>
                                <v-icon color="red">
                                    mdi-close-circle-outline
                                </v-icon>
                            </div>
                        </template>

                        <template v-slot:item.action="{ item }">
                            <v-row>
                                <v-tooltip top>
                                    <template v-if="item.executionType==='update'" v-slot:activator="{ on }">
                                        <v-btn icon v-on="on">
                                            <v-icon medium
                                                    class="mr-2"
                                                    color="primary">
                                                mdi-pencil
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <span>Asset Edited</span>
                                </v-tooltip>

                                <v-tooltip top>
                                    <template v-if="item.executionType==='create'" v-slot:activator="{ on }">
                                        <v-btn icon v-on="on">
                                            <v-icon medium
                                                    class="mr-2"
                                                    color="primary">
                                                mdi-plus
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <span>Asset Created</span>
                                </v-tooltip>

                                <v-tooltip top>
                                    <template v-if="item.executionType==='decommission'" v-slot:activator="{ on }">
                                        <v-btn icon v-on="on">
                                            <v-icon medium
                                                    class="mr-2"
                                                    color="primary">
                                                mdi-archive
                                            </v-icon>
                                        </v-btn>
                                    </template>
                                    <span>Asset Decommissioned</span>
                                </v-tooltip>

                            </v-row>

                        </template>
                    </v-data-table>

                </v-card>
            </v-container>
        </div>
        <v-spacer></v-spacer>
        <a class="noPrint" href="javascript:history.go(-1)"> Go Back</a>
    </v-card>
</template>

<style scoped>
    @media print {

        .noPrint {
            display: none;
        }

        @page {
            size: A4 landscape;
            margin: 12.7mm 7.874mm 10.668mm 9.906mm;
        }

        body {
            margin: 12.7mm 7.874mm 10.668mm 9.906mm;
        }
    }
</style>

<script>

    export default {
        name: 'changePlan-details',
        inject: ['changePlanRepository'],
        props: ['id'],
        data() {
            return {
                loading: false,
                headers: [
                    { text: 'Model Vendor', value: 'newData.Vendor' },
                    { text: 'Model Number', value: 'newData.ModelNumber', },
                    { text: 'Asset Number', value: 'newData.AssetNumber', },
                    { text: 'Hostname', value: 'newData.Hostname' },
                    { text: 'Datacenter', value: 'newData.Datacenter' },
                    { text: 'Rack', value: 'newData.Rack' },
                    { text: 'Rack U', value: 'newData.RackPosition', },
                    { text: 'Owner Username', value: 'newData.Owner' },
                    { text: 'Change', value: 'action', sortable: false },
                ],
                changePlanItems: [],
            }
        },
        computed: {
            getRowsToExpand() {
                var arr = [];
                var index = 0;
                this.changePlanItems.forEach(item => {
                    if (item.executionType === "update") {
                        arr[index] = item;
                        index += 1;
                    }
                })
                return arr;
            }
        },
        async created() {
            this.initialize();
        },
        methods: {
            async initialize() {
                this.changePlanItems = await this.changePlanRepository.listItems(this.id);
                this.deserializeData();
                /*eslint-disable*/
                console.log(this.changePlanItems);
            },
            deserializeData() {
                this.changePlanItems.forEach(item => {
                    item.previousData = JSON.parse(item.previousData);
                    item.newData = JSON.parse(item.newData);
                    if (item.executionType === 'decommission') {
                        item.newData.Vendor = item.previousData.Model.Vendor;
                        item.newData.ModelNumber = item.previousData.Model.Number;
                        item.newData.Rack = item.previousData.Rack.RackLetter + item.previousData.Rack.RackNumber;
                        item.newData.Owner = item.previousData.OwnerName;
                        item.newData.AssetNumber = item.previousData.AssetNumber;
                    }
                });
                console.log(this.changePlanItems);
            },
        }
    }
</script>