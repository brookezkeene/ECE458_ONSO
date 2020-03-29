
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
                        <td :colspan="headers.length">
                            <v-container>
                                <v-simple-table dense fixed-header>
                                    <template v-slot:default>
                                        <thead>
                                            <tr>
                                                <th>Asset</th>
                                                <th>Value</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="(value, name) in getChangePlanItems(item.id)" :key="name" class="text-left">
                                                <td class="font-weight-medium">{{ name }}</td>
                                                <td v-if="isIdProperty(name, item)"><router-link :to="constructLink(value, name, item)">{{ value }}</router-link></td>
                                                <td v-else>{{ value }}</td>
                                            </tr>
                                        </tbody>
                                    </template>
                                </v-simple-table>
                            </v-container>
                        </td>
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
                    { text: 'Model Vendor', value: 'vendor' },
                    { text: 'Model Number', value: 'modelNumber', },
                    { text: 'Asset Number', value: 'assetNumber', },
                    { text: 'Hostname', value: 'hostname' },
                    { text: 'Datacenter', value: 'datacenter' },
                    { text: 'Rack', value: 'rack' },
                    { text: 'Rack U', value: 'rackPosition', },
                    { text: 'Owner Username', value: 'owner' },
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