
<template>
    <v-card flat>
        <v-card-title>Change Plan Details</v-card-title>
        <v-container>
            <v-card>
                <v-spacer></v-spacer>
                <v-data-table :headers="headers"
                              :items="changePlanItems"
                              :search="search"
                              class="elevation-1 pa-10"
                              multi-sort
                              @click:row="showDetails"
                              show-expand>

                    <template v-slot:top>
                        <v-toolbar flat class="mb-6">
                            <v-label>Filter by ... </v-label>
                            <v-spacer></v-spacer>
                            <v-autocomplete prepend-inner-icon="mdi-magnify"
                                            :items="changePlanItems"
                                            :search-input.sync="search"
                                            cache-items
                                            flat
                                            hide-no-data
                                            hide-details
                                            item-text="asset" 
                                            label="Search"
                                            single-line
                                            solo-inverted></v-autocomplete>
                            <v-spacer></v-spacer>
                            <v-btn color="primary" dark class="mb-2" @click="addItem">Add Change Plan</v-btn>
                        </v-toolbar>
                    </template>

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
                                <template v-if="!item.executedDate" v-slot:activator="{ on }">
                                    <v-btn icon v-on="on"
                                           @click="editItem(item)">
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
                                    <v-btn icon v-on="on"
                                           @click="printItem(item)">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-printer
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Print Work Order</span>
                            </v-tooltip>

                            <v-tooltip top>
                                <template v-if="!item.executedDate" v-slot:activator="{ on }">
                                    <v-btn icon v-on="on"
                                           color="primary"
                                           @click="executeItem(item)">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-play-circle
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Execute</span>
                            </v-tooltip>

                            <v-tooltip top>
                                <template v-if="!item.executedDate" v-slot:activator="{ on }">
                                    <v-btn icon v-on="on"
                                           @click="deleteItem(item)">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-delete
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Delete</span>
                            </v-tooltip>
                        </v-row>
                    </template>
                </v-data-table>
            </v-card>
        </v-container>
    </v-card>
</template>


<script>

    export default {
        name: 'changePlan-details',
        inject: ['changePlanRepository'],
        props: ['id'],
        data() {
            return {
                loading: false,
                headers: [
                    { text: 'Model Vendor', value: 'vendor' },
                    { text: 'Model Number', value: 'modelNumber', },
                    { text: 'Hostname', value: 'hostname' },
                    { text: 'Datacenter', value: 'datacenter' },
                    { text: 'Rack', value: 'rack' },
                    { text: 'Rack U', value: 'rackPosition', },
                    { text: 'Owner Username', value: 'owner' },
                    { text: 'Power', value: 'power', sortable: false },
                    { text: 'Actions', value: 'action', sortable: false },
                ],
            }
        },
        async created() {
            this.initialize();
        },
        methods: {
            async initialize() {
                this.changePlanItems = await this.changePlanRepository.listItems(this.id);
            }
        }
    }
</script>