<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">Asset Details</span>
            </v-card-title>
            <v-card-text>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model Vendor</v-label>
                        <v-card-text> {{asset.vendor}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model</v-label>
                        <v-card-text>
                            <router-link :to="{ name: 'model-details', params: { id: asset.modelId } }"> {{ asset.modelNumber }} </router-link>
                        </v-card-text>
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
                        <v-label>Rack Number</v-label>
                        <v-card-text> {{asset.rack}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Rack Position</v-label>
                        <v-card-text> {{asset.rackPosition}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Asset Number</v-label>
                        <v-card-text> {{asset.number}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Owner Username</v-label>
                        <v-card-text v-if="ownerPresent"> {{asset.owner}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Comment</v-label>
                        <v-card-text> {{asset.comment}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <!--MAC Addresses-->
                        <v-label>MAC Addresses</v-label>
                        <v-card flat class="overflow-y-auto">
                            <v-card flat outlined class="overflow-y-auto"  max-height="300px">
                                <div v-for="(port,index) in asset.networkPorts" :key="index">
                                    <v-card-text>{{port.number}} : {{port.pduPort}}</v-card-text>
                                </div>
                            </v-card>
                        </v-card>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <!--network port connections-->
                        <v-label>Network Port Connections</v-label>
                        <v-card flat class="overflow-y-auto">
                            <v-card flat outlined class="overflow-y-auto" max-height="300px">
                                <div v-for="(port,index) in asset.networkPorts" :key="index">
                                    <v-card-text>{{port.number}} : {{port.pduPort}}</v-card-text>
                                </div>
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
                                <div v-for="(port,index) in asset.powerPorts" :key="index">
                                    <v-card-text>{{port.number}} : {{port.pduPort}}</v-card-text>
                                </div>
                            </v-card>
                        </v-card>

                        <v-btn small class="mt-4" color="primary" outlined v-if="!viewPowerPorts" @click="showNames">View Power Port Status</v-btn>
                        <v-btn dark class="mt-4" small color="primary" outlined v-else href @click="hideNames">Hide Power Port Status</v-btn>
                        <div v-if="viewPowerPorts">
                            <v-card max-height="300px" class="overflow-y-auto" flat>
                                <v-card-text v-for="(object,index) in powerPorts.powerPorts" :key="index"> Port {{object.port}}: {{object.status}} </v-card-text>
                            </v-card>
                        </div>
                    </v-col>
                </v-row>
                <v-row v-if="showNeighborhood">
                    <network-neighborhood v-bind:id="asset.id" @click="nodeClicked"></network-neighborhood>
                </v-row>
            </v-card-text>

            <v-spacer />

            <!--Back button to return to main page-->
            <v-spacer></v-spacer>
            <a href="javascript:history.go(-1)"> Go Back</a>

        </v-card>

        <!--<template name="powerPortTable">
            <p>Power Ports</p>
            <v-container fill max-width="50%">
                <v-simple-table dense class="text-center">
                    <thead>
                        <tr>
                            <th class="text-center">Asset Port Number</th>
                            <th class="text-center">Pdu Port</th>
                            <th class="text-center">Power</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(object, index) in asset.powerPorts" :key="index">
                            <td> {{object.number}} </td>
                            <td>{{ object.pduPort }}</td>
                        </tr>
                        <tr v-for="(object, index) in powerPorts.powerPorts" :key="index">
                            <td> {{object.status}} </td>
                        </tr>
                    </tbody>
                </v-simple-table>
            </v-container>
        </template>-->

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
    export default {
        name: 'asset-details',
        inject: ['assetRepository', 'rackRepository'],
        item: null,
        components: {
            NetworkNeighborhood
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
                    modelNumber: '',
                    powerPorts: [],
                },
                ownerPresent: true, // in case the asset does not have an owner, don't need null pointer bc not required.
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
            async initialize() {
                /*eslint-disable*/
                if (!this.loading) this.loading = true;
                this.asset = await this.assetRepository.find(this.id);
                console.log(this.asset);
                this.loading = false;
                if (this.asset.owner === undefined) {
                    this.ownerPresent = false;
                }
            },
            async fetchPowerPortIds() {
                var powerPortStates = [];
                /*eslint-disable*/
                powerPortStates = await this.assetRepository.getPowerPortState(this.asset.id);
                console.log(powerPortStates);
                for (var i = 0; i<powerPortStates.powerPorts.length; i++) {
                    if (powerPortStates.powerPorts[i].status=='0') {
                        powerPortStates.powerPorts[i].status = 'On';
                    } else {
                        powerPortStates.powerPorts[i].status = 'Off'
                    }
                    var name = powerPortStates.powerPorts[i].port.split('-');
                    console.log(name);
                    powerPortStates.powerPorts[i].port = name[2][3] + " " + name[2][4];
                }
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
            nodeClicked(e) {
                /* eslint-disable no-unused-vars, no-console */
                console.log('clicked');
                this.$router.push({ name: 'asset-details', params: { id: e } });
            }

        }
    }
</script>