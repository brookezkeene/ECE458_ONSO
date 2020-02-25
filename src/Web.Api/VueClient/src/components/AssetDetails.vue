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
                    <v-col cols="12" sm="6" md="4"> <!--power port connections-->>
                        <v-label>Power Port Connections: </v-label>
                        <v-card-text>{{asset.powerPorts}}</v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Power Ports: </v-label>
                        <a v-if="!viewNames" href="#" @click="showNames">View Power Port Status</a>
                        <a v-else href="#" @click="hideNames">Hide Power Port Status</a>
                        <div v-if="viewNames">
                            <v-card max-height="300px" class="overflow-y-auto" outlined="true" flat>
                                <v-card-text v-for="port in powerPorts" :key="port"> Port {{port.number}}: {{port.status}} </v-card-text>
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
                powerPorts: [],
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
                console.log(this.asset.powerPorts.length);
                console.log('Got to fetch power port ids!');
                for (i = 0; i < this.asset.powerPorts.length; i++) {
                    if (!this.loading) this.loading = true;
                    powerPortStates.put(await this.assetRepository.findPowerPort(this.asset.powerPorts[i].id));
                    console.log(powerPortStates);
                    this.loading = false;
                }
                console.log(powerPortStates);
            },
            showNames() {
                this.powerPorts = this.fetchPowerPortIds();
                this.viewNames = true;
            },
            hideNames() {
                this.viewNames = false;
            }            
        }
    }
</script>