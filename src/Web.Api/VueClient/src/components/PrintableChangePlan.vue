﻿
<template>
    <v-card flat id="card">
        <v-card-title>
            <span>Work Order Summary:</span>
            <span v-if="$store.getters.changePlan.name">
                {{$store.getters.changePlan.name}} in datacenter {{$store.getters.changePlan.datacenterName}}
            </span>
        </v-card-title>
        <div id='print'>
            <v-container>
                <v-layout child-flex>
                    <v-card flat>
                        <v-spacer></v-spacer>
                        <v-data-table :headers="headers"
                                      :items="changePlanItems"
                                      multi-sort
                                      show-expand
                                      :expanded="getRowsToExpand"
                                      disable-pagination
                                      hide-default-footer
                                      class="scaled">

                            <template v-slot:item.data-table-expand="{ item, isExpanded, expand }">
                                <v-btn id="noPrint" icon v-if="isExpanded"><v-icon>mdi-chevron-up</v-icon></v-btn>
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
                                <td v-if="item.executionType=='update'">{{item.previousData.powerConnections}}</td>
                                <td v-if="item.executionType=='update'">{{item.previousData.macAddresses}}</td>
                                <td v-if="item.executionType=='update'">{{item.previousData.networkConnections}}</td>

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
                </v-layout>
            </v-container>
        </div>
        <v-spacer></v-spacer>
        <a class="noPrint" href="javascript:history.go(-1)"> Go Back</a>
    </v-card>
</template>

<style scoped>
    @media print {

        .scaled {
          transform: scale(0.9); /* Equal to scaleX(0.7) scaleY(0.7) */
        }

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
                    { text: 'Model No.', value: 'newData.ModelNumber', },
                    { text: 'Asset No.', value: 'newData.AssetNumber', },
                    { text: 'Hostname', value: 'newData.Hostname' },
                    { text: 'Center', value: 'newData.Datacenter' },
                    { text: 'Rack', value: 'newData.Rack' },
                    { text: 'Rack U', value: 'newData.RackPosition', },
                    { text: 'Owner Username', value: 'newData.Owner' },
                    { text: 'Power Ports', value: 'newData.powerConnections' },
                    { text: 'Network Port Mac Addresses', value: 'newData.macAddresses' },
                    { text: 'Network Port Connections', value: 'newData.networkConnections' },
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
                console.log(this.changePlanItems);
                this.changePlanItems.forEach(item => {
                    item.previousData = JSON.parse(item.previousData);
                    item.newData = JSON.parse(item.newData);
                    if (item.executionType === 'decommission') {
                        item.newData.Vendor = item.previousData.Model.Vendor;
                        item.newData.ModelNumber = item.previousData.Model.Number;
                        item.newData.Rack = item.previousData.Rack.RackLetter + item.previousData.Rack.RackNumber;
                        item.newData.Owner = item.previousData.OwnerName;
                        item.newData.AssetNumber = item.previousData.AssetNumber;
                    } else {
                        var index = 0;
                        if (item.newData != null) {
                            item.newData.macAddresses = [];
                            item.newData.networkConnections = [];
                            item.newData.powerConnections = [];
                            // New Network Port Data 
                            item.newData.NetworkPorts.forEach(networkPort => {
                                if (item.newData.macAddresses[index] != undefined) {
                                    item.newData.macAddresses[index] = networkPort.Name + ": " + networkPort.MacAddress + " ";
                                }
                                if (networkPort.ConnectedPort != undefined) {
                                    item.newData.networkConnections[index] = networkPort.Name + ": Host " + networkPort.ConnectedPort.Hostname + " Port " + networkPort.ConnectedPort.Name;
                                }
                                index++;
                            });

                            var index4 = 0;
                            // New Power Ports
                            item.newData.PowerPorts.forEach(powerPort => {
                                if (powerPort.PduPort != undefined) {
                                    item.newData.powerConnections[index4] = powerPort.Number + ": " + powerPort.PduPort + " ";
                                }
                                index4++;
                            })
                        }
                        if (item.previousData != null) {
                            item.previousData.macAddresses = [];
                            item.previousData.networkConnections = [];
                            item.previousData.powerConnections = [];

                            // Previous Network Port Data
                            var index2 = 0;
                            item.previousData.NetworkPorts.forEach(networkPort => {
                                if (item.previousData.macAddresses[index2] != undefined) {
                                    item.previousData.macAddresses[index2] = networkPort.Name + ": " + networkPort.MacAddress + " ";
                                }
                                if (networkPort.ConnectedPort != undefined) {
                                    item.previousData.networkConnections[index2] = networkPort.Name + ": Host " + networkPort.ConnectedPort.Hostname + " Port " + networkPort.ConnectedPort.Name;
                                }
                                index2++;
                            })
                            var index3 = 0;
                            // Previous Power Ports
                            item.previousData.PowerPorts.forEach(powerPort => {
                                if (powerPort.PduPort != undefined) {
                                    item.previousData.powerConnections[index3] = powerPort.Number + ": " + powerPort.PduPort + " ";
                                }
                                index3++;
                            })
                        }
                    }
                });
                console.log(this.changePlanItems);
            },
        }
    }
</script>