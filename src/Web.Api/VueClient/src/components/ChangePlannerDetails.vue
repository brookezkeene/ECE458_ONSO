
<template>
    <v-card flat>
        <v-card-title>Change Plan Details</v-card-title>
        <v-container>
            <v-card>
                <v-spacer></v-spacer>
                <v-data-table :headers="headers"
                              :items="changePlanItems"
                              class="pa-5"
                              multi-sort
                              show-expand>

                    <template v-slot:item.data-table-expand="{ item, isExpanded, expand }">
                        <v-btn icon @click="expand(true)" v-if="item.executionType=='update' && !isExpanded"><v-icon>mdi-chevron-down</v-icon></v-btn>
                        <v-btn icon @click="expand(false)" v-if="isExpanded"><v-icon>mdi-chevron-up</v-icon></v-btn>
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

                <div class="text-center">
                    <v-btn v-if="!executedDate" color="primary" dark class="mb-2" @click="execute">Execute Change Plan</v-btn>
                </div>
            </v-card>
        </v-container>
        <v-spacer></v-spacer>
        <a href="javascript:history.go(-1)"> Go Back</a>
    </v-card>
</template>


<script>

    export default {
        name: 'changePlan-details',
        inject: ['changePlanRepository', 'assetRepository'],
        props: ['id', 'executedDate'],
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
                    { text: 'Power Ports', value: 'newData.powerConnections' },
                    { text: 'Network Port Mac Addresses', value: 'newData.macAddresses' },
                    { text: 'Network Port Connections', value: 'newData.networkConnections' },
                    { text: 'Change', value: 'action', sortable: false },
                ],
                changePlanItems: [],
            }
        },
        async created() {
            this.initialize();
        },
        methods: {
            async initialize() {
                this.changePlanItems = await this.changePlanRepository.listItems(this.id);
                this.deserializeData(this.changePlanItems);
                /*eslint-disable*/
                console.log(this.changePlanItems);
            },
            deserializeData(changePlanItems) {
                console.log(changePlanItems);
                changePlanItems.forEach(item => {
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
                                item.newData.macAddresses[index] = networkPort.Name + ": " + networkPort.MacAddress +" " ;
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
                                item.newData.powerConnections[index4] = powerPort.Number + ": " + powerPort.PduPort +" ";
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
                                    item.previousData.powerConnections[index3] = powerPort.Number + ": " + powerPort.PduPort +" ";
                                }
                                index3++;
                            })
                        }
                    }
                });
                console.log(this.changePlanItems);
            },
            execute() {
                (confirm('Are you sure you want to execute this change plan?') && this.changePlanRepository.execute(this.$store.getters.changePlan.id))
                    .then( () => {
                        this.$router.push({ name: 'change-plan' })
                    });
            },
        }
    }
</script>