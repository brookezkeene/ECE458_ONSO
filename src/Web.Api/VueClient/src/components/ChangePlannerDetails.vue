
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
                        <v-btn icon @click="expand(true)" v-if="item.previousData!=null && !isExpanded"><v-icon>mdi-chevron-down</v-icon></v-btn>
                        <v-btn icon @click="expand(false)" v-if="isExpanded"><v-icon>mdi-chevron-up</v-icon></v-btn>
                    </template>

                    <template v-slot:expanded-item="{ headers, item }">
                            <td v-if="item.previousData!=null"></td>
                            <td v-if="item.previousData!=null">{{item.previousData.Vendor}}</td>
                            <td v-if="item.previousData!=null">{{item.previousData.ModelNumber}}</td>
                            <td v-if="item.previousData!=null">{{item.previousData.AssetNumber}}</td>
                            <td v-if="item.previousData!=null">{{item.previousData.Hostname}}</td>
                            <td v-if="item.previousData!=null">{{item.previousData.Datacenter}}</td>
                            <td v-if="item.previousData!=null">{{item.previousData.Rack}}</td>
                            <td v-if="item.previousData!=null">{{item.previousData.RackPosition}}</td>
                            <td v-if="item.previousData!=null">{{item.previousData.Owner}}</td>

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

                        </v-row>

                    </template>
                </v-data-table>

                <div class="text-center">
                    <v-btn color="primary" dark class="mb-2" @click="execute">Execute Change Plan</v-btn>
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
        inject: ['changePlanRepository','assetRepository'],
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
            execute(item) {
                (confirm('Are you sure you want to execute this change plan?') && this.changePlanRepository.execute(item))
                    .then( () => {
                        this.$router.push({ name: 'change-plan' })
                    });
            },
        }
    }
</script>