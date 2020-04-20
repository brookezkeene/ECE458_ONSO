
<template>
    <v-card flat id="card">
        <v-card-title>
            <span>Work Order Summary:</span>
            <span v-if="$store.getters.changePlan.name">
                {{$store.getters.changePlan.name}} in datacenter {{$store.getters.changePlan.datacenterName}}
            </span>
        </v-card-title>
        <div id='print'>
            <v-container v-for="(item, index) in changePlanItems" :key="index">
                <div v-if="item.executionType === 'create'">
                    <h4>Create an Asset with:</h4>
                    <ul>
                        <li>
                            Vendor and Model Number: {{item.newData.Vendor}} {{item.newData.ModelNumber}}
                        </li>
                        <li>
                            Hostname: {{item.newData.Hostname}}
                        </li>
                        <li v-if="item.newData.MountType !== 'blade'">
                            Rack, Rack U: {{item.newData.Rack}}, {{item.newData.RackPosition}}
                        </li>
                        <li v-else>
                            Chassis, Slot: {{item.newData.ChassisId}}, {{item.newData.ChassisSlot}}
                        </li>
                        <li>
                            With MAC Addresses:
                            <ul>
                                <li v-for="(port, index) in macAddresses" :key="index">

                                </li>
                            </ul>
                        </li>
                        <li>
                            With Network Connections:
                            <ul>
                                <li v-for="(port, index) in networkConnections" :key="index">

                                </li>
                            </ul>
                        </li>
                        <li>
                            With Power Connections:
                            <ul>
                                <li v-for="(port, index) in powerConnections" :key="index">
                                    
                                </li>
                            </ul>
                        </li>
                    </ul>
                </div>
                <div v-if="item.executionType === 'update'">
                    <h4>Update Asset with hostname {{item.previousData.Hostname}} and asset number {{item.previousData.AssetNumber}} the following changes:</h4>
                    <ul>
                        <li>
           
                        </li>
                    </ul>
                </div>
                <div v-if="item.executionType === 'decommission'">
                    <h4>Decommission the following Asset:</h4>
                    <ul>
                        <li>
                            Hostname: {{item.previousData.Hostname}}
                        </li>
                        <li>
                            Asset Number: {{item.previousData.AssetNumber}}
                        </li>
                        <li v-if="item.previousData.MountType !== 'blade'">
                            Rack: {{item.previousData.Rack}}
                        </li>
                        <li v-else>
                            Chassis: {{item.previousData.ChassisId}}
                        </li>
                        <li v-if="item.previousData.MountType !== 'blade'">
                            Rack U: {{item.previousData.RackPosition}}
                        </li>
                        <li v-else>
                            Slot: {{item.previousData.ChassisSlot}}
                        </li>
                    </ul>
                </div>
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
            size: A4 portrait;
            margin: 12.7mm 7.874mm 10.668mm 9.906mm;
        }

        body {
            margin: 12.7mm 7.874mm 10.668mm 9.906mm;
        }
    }
</style>

<script>
    //import workOrder from '@/workOrder';

    export default {
        name: 'changePlan-details',
        inject: ['changePlanRepository'],
        props: ['id'],
        data() {
            return {
                loading: false,
                changePlanItems: [],
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
                        //item.newData.Vendor = item.previousData.Model.Vendor;
                        //item.newData.ModelNumber = item.previousData.Model.Number;
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
                    if (item.executionType === 'update') {
                        var changes = [];
                        for (var field in item.newData) {
                            console.log(field)
                        }
                        item.changes = changes;
                    }
                });
                console.log(this.changePlanItems);
            },
            isolateChanges() {
                for (var item in this.changePlanItems) {

                }
            },
        }
    }
</script>