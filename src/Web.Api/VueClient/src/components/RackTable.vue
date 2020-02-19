<template>
    <v-card>
        <v-data-table
          :headers="filteredHeaders"
          :items="racks"
          multi-sort
          >
            <template v-slot:top>

                <v-toolbar flat color="white">
                    <v-toolbar-title>Existing Racks from </v-toolbar-title>

                    <v-select v-model="selectedDatacenter"
                              :items="datacenters"
                              item-text="name"
                              item-value=""
                              :return-object="false"
                              label="Datacenter"
                              placeholder="Select a datacenter or all datacenters"
                              class="pt-8 pl-4"
                              clearable
                              @change="datacenterSearch()">
                    </v-select>
                    <v-spacer></v-spacer>
                    <v-spacer></v-spacer>
                </v-toolbar>

            </template>

            <template v-slot:item.action="{ item }">
                <v-row class="pl-5">
                    <v-icon medium
                            @click="deleteItem(item)">mdi-delete</v-icon>
                </v-row>
            </template>

            <template v-slot:no-data>
                <v-btn color="primary" @click="initialize">Refresh</v-btn>
            </template>
        </v-data-table>
    </v-card>
</template>

<script>
    import Auth from "../auth"

    export default {
        name: 'rack-table',
        inject: ['rackRepository', 'datacenterRepository'],
        item: null,
        data () {
            return {
                loading: true,
                headers: [
                    { text: 'Address', value: 'address'},
                    { text: 'Row Letter', value: 'rowLetter' },
                    { text: 'Rack Number', value: 'rackNumber' },
                    { text: 'Datacenter', value: 'address' }, // change this to rack's datacenter field
                    { text: 'Actions', value: 'action', sortable: false },
                ],
                racks: [],
                datacenters: [],
                selectedDatacenter: ''
            };
        },
        computed: {
            admin() {
                return Auth.isAdmin()
            },
            filteredHeaders() {
                return (this.admin) ? this.headers : this.headers.filter(h => h.text !== "Actions")
            },
        },
        async created () {
            this.initialize()
        },
        methods: {
            async initialize() {
                this.racks = await this.rackRepository.list();
                this.datacenters = await this.datacenterRepository.list();
                var datacenter = {
                    name: "All Datacenters",
                    abbreviation: "All",
                }
                this.datacenters.push(datacenter);
                this.loading = false;
            },
            deleteItem (item) {
                confirm('Are you sure you want to delete this item?') && this.rackRepository.deleteInRange(item.address, item.address)
            },
            async datacenterSearch() {
                if (this.selectedDatacenter === "All Datacenters") {
                    // make special request for all datacenter racks
                } else {
                    this.racks = await this.rackRepository.list() // re-call based on new datacenter Name

                }
            },
        }
    }
</script>