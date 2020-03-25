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

                    <template v-slot:item.action="{ item }">
                        <v-row>
                            <v-tooltip top>
                                <template v-slot:activator="{ on }">
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
                                <template v-slot:activator="{ on }">
                                    <v-btn icon v-on="on"
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
                                <template v-slot:activator="{ on }">
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
    inject: ['assetRepository',],
    data() {
        return {
        loading: true,
        search: '',
        // TODO: replace with change plan data
        headers: [
            { text: 'Change Plan Name', value: 'vendor' },
            { text: 'Executed', value: 'modelNumber', },
            { text: 'Date & Time', value: 'hostname' },
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
            // TODO: replace with change plan data
            this.assets = await this.assetRepository.list();

            this.loading = false;

        },
        deleteItem (item) {
            this.editing = true;
            console.log("delete" + item);
            confirm('Are you sure you want to delete this asset?') //&& this.assetRepository.delete(item)
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
            this.$router.push({ name: 'change-plan-edit', params: { id: item.id } })
        },
        addItem() {
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
    },
  }
</script>