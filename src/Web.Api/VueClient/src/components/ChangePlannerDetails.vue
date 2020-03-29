
<template>
    <v-card flat>
        <v-card-title>Change Plan Details</v-card-title>
        <v-container>
            <v-card>
                <v-spacer></v-spacer>
                <v-data-table :headers="headers"
                              :items="changePlanItems"
                              class="pa-10"
                              multi-sort
                              show-expand>

                    <template v-slot:expanded-item="{ headers, item }">
                        <td></td>
                        <td>{{item.previousData.Vendor}}</td>
                        <td>{{item.previousData.ModelNumber}}</td>
                        <td>{{item.previousData.AssetNumber}}</td>
                        <td>{{item.previousData.Hostname}}</td>
                        <td>{{item.previousData.Datacenter}}</td>
                        <td>{{item.previousData.Rack}}</td>
                        <td>{{item.previousData.RackPosition}}</td>
                        <td>{{item.previousData.Owner}}</td>
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
                                <template v-slot:activator="{ on }">
                                    <v-btn icon v-on="on">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-pencil
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Edit</span>
                            </v-tooltip>
                            <v-tooltip top>
                                <template v-slot:activator="{ on }">
                                    <v-btn icon v-on="on">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-plus
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Edit</span>
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
                    { text: 'Actions', value: 'action', sortable: false },
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