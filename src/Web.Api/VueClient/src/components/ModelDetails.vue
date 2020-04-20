<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">Model Details</span>
            </v-card-title>
            <!-- Contains the model data-->
            <v-card-text>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model </v-label>
                        <v-card-text> {{model.vendor}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model Number </v-label>
                        <v-card-text> {{model.modelNumber}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Mount Type </v-label>
                        <v-card-text> {{typeMap[model.mountType]}} </v-card-text>
                    </v-col>
                    <v-col v-if="!isBlade"
                           cols="12" sm="6" md="4">
                        <v-label>Height </v-label>
                        <v-card-text> {{model.height}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>CPU </v-label>
                        <v-card-text> {{model.cpu}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Memory </v-label>
                        <v-card-text> {{model.memory}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Storage </v-label>
                        <v-card-text> {{model.storage}} </v-card-text>
                    </v-col>
                    <v-col v-if="!isBlade"
                           cols="12" sm="6" md="4">
                        <v-label>Power Ports </v-label>
                        <v-card-text> {{model.powerPorts}} </v-card-text>
                    </v-col>
                    <v-col v-if="!isBlade"
                           cols="12" sm="6" md="4">
                        <v-label>Network Ports </v-label>
                        <v-card-text> {{model.ethernetPorts}} </v-card-text> <!--networkPorts-->

                        <v-btn small color="primary" outlined v-if="!viewNames" @click="showNames">View Network Port Names</v-btn>
                        <v-btn dark small color="primary" outlined v-else href @click="hideNames">Hide Network Port Names</v-btn>

                        <div v-if="viewNames">
                            <v-card max-height="300px" class="overflow-y-auto" outlined flat>
                                <v-card-text v-for="(port, index) in model.networkPorts" :key="index"> Port {{port.number}}: {{port.name}} </v-card-text>
                            </v-card>
                        </div>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Display Color </v-label>
                        <v-card-text>
                            {{model.displayColor}}
                            <v-icon class="mr-2"
                                    :color=model.displayColor>
                                mdi-circle
                            </v-icon>
                        </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Comment </v-label>
                        <v-textarea :value="model.comment" disabled>  </v-textarea>
                    </v-col>
                </v-row>
  
                <!--Links to the assets for this model-->
                <div>
                    <v-label>Assets</v-label>
                </div>
                <v-container fill max-width="50%" >
                    <v-simple-table dense class="text-center">
                        <thead>
                            <tr>
                                <th class="text-center">Asset Number</th>
                                <th class="text-center">Host Name</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(item, index) in model.assets" :key="index">
                                <td @click="goToAssetDetails(item.id)">
                                    {{item.assetNumber}}
                                    <v-icon small>mdi-open-in-new</v-icon>
                                </td>
                                <td @click="goToAssetDetails(id)">{{ item.hostname }}</td>
                            </tr>
                        </tbody>
                    </v-simple-table>
                </v-container>
            </v-card-text>
        </v-card>

        <v-spacer />

        <!--Back button to return to main page-->
        <v-spacer></v-spacer>
        <a @click="back">Go Back</a>
    </div>
</template>

<style>
    .v-label{
        color: #4bbd51;
    }
</style>

<script>

    export default {
        name: 'model-details',
        inject: ['modelRepository'],
        props: ['id'],
        data() {
            return {
                loading: false,
                model: {
                    vendor: '',
                    modelNumber: 'foo',
                },
                networkPorts: [ ],
                viewNames: false,
                typeMap: {
                    "normal" : "Normal", 
                    "chassis" : "Blade Chassis", 
                    "blade" : "Blade"
                },
            };
        },
        created() {
            this.fetchModel();
        },
        watch: {
            id: function () {
                this.fetchModel();
            }
        },
        computed: {
            isBlade() {
                return this.model.mountType === 'blade'
            }
        },
        methods: {
            async fetchModel() {
                if (!this.loading) this.loading = true;
                this.model = await this.modelRepository.find(this.id);
                this.loading = false;
                /*eslint-disable*/
                console.log(this.model);
            },
            goToAssetDetails(id) {
                this.$router.push({ name: 'asset-details', params: { id: id } });
            },
            showNames() {
                this.viewNames = true;
            },
            hideNames() {
                this.viewNames = false;
            },
            back() {
                this.$router.go(-1);
            }
        }
    }</script>