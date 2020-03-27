<template>
    <v-card flat>
        <v-card-title>Change Planner</v-card-title>
        <v-container>
            <v-card>
                <v-spacer></v-spacer>
                <!--TODO: change for change plans -->
                <v-data-table :headers="headers"
                              :items="assets"
                              :search="search"
                              class="elevation-1 pa-10"
                              multi-sort
                              show-expand
                              @click:row="showDetails">

                    <template v-slot:top>
                        <v-toolbar flat class="mb-6">
                            <v-label>Filter by ... </v-label>
                            <v-spacer></v-spacer>
                            <v-autocomplete prepend-inner-icon="mdi-magnify"
                                            :items="assets"
                                            :search-input.sync="search"
                                            cache-items
                                            flat
                                            hide-no-data
                                            hide-details
                                            item-text="vendor"
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
    inject: ['assetRepository','changePlanRepository'],
    data() {
        return {
        loading: true,
        search: '',
        // TODO: replace with change plan data
        headers: [
            { text: 'Change Plan Name', value: 'name' },
            { text: 'Executed', value: 'executed', },
            { text: 'Date & Time', value: 'createdDate' },
            { text: 'Actions', value: 'action', sortable: false },
        ],
        assets: [],
        defaultItem: {
            id: '',
            datacenter: '',
            modelId: '',
            hostname: '',
            rackId: '',
            rackPosition: '',
            ownerId: '',
            comment: '',
            poweredOn: false,
        },
        editing: false,
        }
    },
    async created() {
      this.initialize()
    },

    methods: {
        /*eslint-disable*/
        async initialize() {
            this.assets = await this.changePlanRepository.list(this.$store.getters.userId); //only list change plans for a given user
            console.log(this.assets);
            this.loading = false;
        },
        deleteItem (item) {
            this.editing = true;
            console.log("delete" + item);
            confirm('Are you sure you want to delete this asset?') && this.changePlanRepository.delete(item)
                    .then(async () => {
                        await this.initialize();
                    })
        },
        printItem (item) {
            //TODO: print work order code
            console.log("print" + item);
        },
        executeItem(item) {
            //TODO: execute change plan code
            console.log("execute" + item);
        },
        editItem(item) {
            this.editing = true;
            //TODO: edit change plan code, change this probably
            this.$store.dispatch('startChangePlan', item.name);
            this.$router.push({ name: 'assets'})
        },
        addItem(item) {
            //TODO: adde new change plan code, change this probably
            
            this.$router.push({ name: 'change-plan-new' })
        },
        showDetails(item) {
            //TODO: show change plan summary code
            if (!this.editing) {
                console.log(item);
                this.$router.push({ name: 'change-plan-details', params: {id: item.id } })
            }
            this.editing = false;
        },
        async getChangePlanItems(changeplanId) {
            var items = await this.changePlanRepository.listItems(changeplanId);
            /*eslint-disable*/
            console.log(items);
            return items;
        }
    },
  }
</script>