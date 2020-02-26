<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">Asset Details</span>
            </v-card-title>
            <v-card-text>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Data Center: </v-label>
                        <v-card-text> {{asset.datacenter}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Host Name: </v-label>
                        <v-card-text> {{asset.hostname}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Rack Number: </v-label>
                        <v-card-text> {{asset.rack}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Rack Position: </v-label>
                        <v-card-text> {{asset.rackPosition}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Owner Username: </v-label>
                        <v-card-text v-if="ownerPresent"> {{asset.owner}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Comment: </v-label>
                        <v-card-text> {{asset.comment}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model Vendor: </v-label>
                        <v-card-text> {{asset.vendor}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model: </v-label>
                        <router-link :to="{ name: 'model-details', params: { id: asset.modelId } }"> {{ asset.modelNumber }} </router-link>
                    </v-col>
                    <v-col cols="12" sm="6" md="4"> <!--power port connections-->
                        <v-label>Power Port Connections: </v-label>
                        <v-scroll>
                            <v-card flat class="overflow-y-auto">
                                <div v-for="(port,index) in asset.powerPorts" :key="index">
                                    <v-card-text>{{port.pduPort}}</v-card-text>

                                </div>
                            </v-card>
                        </v-scroll>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-btn small color="primary" v-if="!viewNames" @click="showNames">View Power Port Status</v-btn>
                        <v-btn dark small color="primary" v-else href @click="hideNames">Hide Power Port Status</v-btn>
                        <div v-if="viewNames">
                            <v-card max-height="300px" class="overflow-y-auto" flat>
                                <v-card-text v-for="(object,index) in powerPorts.powerPorts" :key="index"> Port {{object.port}}: {{object.status}} </v-card-text>
                            </v-card>
                        </div>
                    </v-col>
                </v-row>

            </v-card-text>

            <v-spacer />

            <!--Back button to return to main page-->
            <v-spacer></v-spacer>
            <a href="javascript:history.go(-1)"> Go Back</a>

        </v-card>
    </div>
</template>

<script>
    export default {
        name: 'asset-details',
        inject: ['assetRepository'],
        item: null,
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
                powerPorts: {},
            };
        },
        created() {
            this.fetchasset();
        },
        methods: {
            async fetchasset() {
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
                console.log(powerPortStates);
                console.log('Got to fetch power port ids!');
                powerPortStates = await this.assetRepository.getPowerPortState(this.asset.id);
                console.log(powerPortStates);
                for (var i = 0; i<powerPortStates.powerPorts.length; i++) {
                    console.log(powerPortStates.powerPorts[i]);
                    if (powerPortStates.powerPorts[i].status=='0') {
                        powerPortStates.powerPorts[i].status = 'On';
                        var name = powerPortStates.powerPorts[i].port.split('-');
                        console.log(name);
                        powerPortStates.powerPorts[i].port = name[2][3] + " " + name[2][4];
                    } else {
                        powerPortStates.powerPorts[i].status = 'Off'
                    }
                }
                return powerPortStates;
            },
            async showNames() {
                this.powerPorts = await this.fetchPowerPortIds();
                console.log(this.powerPorts);
                this.viewNames = true;
            },
            hideNames() {
                this.viewNames = false;
            }            
        }
    }
</script>