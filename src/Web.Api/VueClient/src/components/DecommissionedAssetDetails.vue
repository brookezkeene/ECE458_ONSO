<template>
    <div v-if="!loading">
        <v-card flat>
            <ChangePlanBar></ChangePlanBar>
            <v-card-title>
                <span class="headline">Decommissioned Asset Details</span>
            </v-card-title>
            <v-card-text>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Time Decommissioned</v-label>
                        <v-card-text> {{asset.dateDecommissioned}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Decommissioned By</v-label>
                        <v-card-text> {{asset.decommissioner}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model Vendor</v-label>
                        <v-card-text> {{asset.modelName}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model Number</v-label>
                        <v-card-text> {{ asset.modelNumber }} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Host Name</v-label>
                        <v-card-text> {{asset.hostname}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Data Center</v-label>
                        <v-card-text> {{asset.datacenter}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Rack</v-label>
                        <v-card-text> {{asset.rackAddress}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Rack Position</v-label>
                        <v-card-text> {{asset.data.RackPosition}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Asset Number</v-label>
                        <v-card-text> {{asset.data.AssetNumber}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Owner Username</v-label>
                        <v-card-text v-if="ownerPresent"> {{asset.data.OwnerName}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Comment</v-label>
                        <v-textarea :value="asset.data.Comment" disabled>  </v-textarea>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <!--MAC Addresses-->
                        <v-label>MAC Addresses</v-label>
                        <v-card flat class="overflow-y-auto">
                            <v-card flat outlined class="overflow-y-auto" max-height="300px">
                                <v-simple-table dense>
                                    <template v-slot:default>
                                        <thead>
                                            <tr>
                                                <th class="text-left">Name</th>
                                                <th class="text-left">MAC</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="port in asset.data.NetworkPorts" :key="port.id">
                                                <td>{{ port.Name }}</td>
                                                <td>{{ port.MacAddress }}</td>
                                            </tr>
                                        </tbody>
                                    </template>
                                </v-simple-table>
                            </v-card>
                        </v-card>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <!--network port connections-->
                        <v-label>Network Port Connections</v-label>
                        <v-card flat class="overflow-y-auto">
                            <v-card flat outlined class="overflow-y-auto" max-height="300px">
                                <v-simple-table dense>
                                    <template v-slot:default>
                                        <thead>
                                            <tr>
                                                <th class="text-left">Name</th>
                                                <th class="text-left">Hostname</th>
                                                <th class="text-left">Name</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="port in asset.data.NetworkPorts" :key="port.id">
                                                <td>{{ port.Name }}</td>
                                                <td>{{ port.ConnectedPort && port.ConnectedPort.Hostname }}</td>
                                                <td>{{ port.ConnectedPort && port.ConnectedPort.Name }}</td>
                                            </tr>
                                        </tbody>
                                    </template>
                                </v-simple-table>
                            </v-card>
                        </v-card>

                        <v-btn small class="mt-4" color="primary" outlined v-if="!showNeighborhood" @click="showNeighborhood = true">View Network Neighborhood</v-btn>
                        <v-btn small class="mt-4" color="primary" outlined v-if="showNeighborhood" @click="showNeighborhood = false">Hide Network Neighborhood</v-btn>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <!--power port connections-->
                        <v-label>Power Port Connections</v-label>
                        <v-card flat class="overflow-y-auto">
                            <v-card flat outlined class="overflow-y-auto">
                                <v-simple-table dense>
                                    <template v-slot:default>
                                        <thead>
                                            <tr>
                                                <th class="text-left">#</th>
                                                <th class="text-left">Port</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="port in asset.data.PowerPorts" :key="port.id">
                                                <td>{{ port.Number }}</td>
                                                <td>{{ port.PduPort }}</td>
                                            </tr>
                                        </tbody>
                                    </template>
                                </v-simple-table>
                            </v-card>
                        </v-card>
                    </v-col>
                </v-row>
                <v-row v-if="showNeighborhood">
                    <network-neighborhood v-bind:id=undefined v-bind:networkJson="asset.data.NetworkPortGraph"></network-neighborhood>
                </v-row>
            </v-card-text>

            <v-spacer />

            <!--Back button to return to main page-->
            <v-spacer></v-spacer>
            <a href="javascript:history.go(-1)"> Go Back</a>

        </v-card>
    </div>

</template>

<style>
    .v-label {
        font-size: 20px;
    }

    .p {
        font-size: 15px;
    }
</style>

<script>
    import NetworkNeighborhood from "./NetworkNeighborhood"
    import ChangePlanBar from './ChangePlanStatusBar';
    export default {
        name: 'decommissioned-asset-details',
        inject: ['assetRepository', 'rackRepository'],
        item: null,
        components: {
            NetworkNeighborhood,
            ChangePlanBar
        },
        props: ['id'],
        data() {
            return {
                loading: false,
                asset: {
                    id:'',
                    hostname: '',
                    rack: '',
                    rackPosition: '',
                    owner: '',
                    comment: '',
                    vendor: '',
                    modelNumber: ''
                },
                ownerPresent: true, // in case the asset does not have an owner, don't need null pointer bc not a required field.
                viewNames: false,
                viewPowerPorts: false,
                powerPorts: {},
                showNeighborhood: false
            };
        },
        created() {
            this.initialize();
        },
        methods: {
            changePlanId() {
                if (this.$store.getters.isChangePlan)
                    return this.$store.getters.changePlan.id;
            },
            async initialize() {
                /*eslint-disable*/
                if (!this.loading) this.loading = true;

                const asset = await this.assetRepository.getDecommissionedAsset(this.id, this.changePlanId());
                

                this.loading = false;
                if (asset.owner === undefined) {
                    this.ownerPresent = false;
                }

                // Turn data blob into fields to be read in table
                asset.data = JSON.parse(asset.data);

                asset.data.NetworkPorts = asset.data.FullNetworkPorts;
                asset.data.PowerPorts.forEach(port => port.status = undefined);
                asset.data.NetworkPorts.sort((a, b) => a.Number - b.Number);
                asset.data.PowerPorts.sort((a, b) => a.Number - b.Number);

                this.asset = asset;
            },
            async fetchPowerPortIds() {
                var powerPortStates = [];
                /*eslint-disable*/
                powerPortStates = await this.assetRepository.getPowerPortState(this.asset.id);
                console.log(powerPortStates);
                for (var i = 0; i<powerPortStates.powerPorts.length; i++) {
                    if (powerPortStates.powerPorts[i].status=='0') {
                        powerPortStates.powerPorts[i].status = 'on';
                    } else {
                        powerPortStates.powerPorts[i].status = 'off'
                    }
                }
                this.asset.PowerPorts.forEach(port => {
                    var maybeState = powerPortStates.powerPorts.find(o => o.port === port.pduPort);
                    port.status = maybeState && maybeState.status;
                })
                return powerPortStates;
            },
            async showNames() {
                this.powerPorts = await this.fetchPowerPortIds();
                console.log(this.powerPorts);
                this.viewPowerPorts = true;
            },
            hideNames() {
                this.viewPowerPorts = false;
            },

        }
    }
</script>