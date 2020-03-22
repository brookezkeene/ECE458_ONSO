<template>
    <v-card>
        <v-data-table
          :headers="filteredHeaders"
          :items="racks"
          :options.sync="options"
          :server-items-length="totalItems"
          class="pa-10"
          >
            <template v-slot:top>

                <v-toolbar flat color="white">
                    <v-toolbar-title>Existing Racks from </v-toolbar-title>

                    <v-select v-model="selectedDatacenter"
                              :items="datacenters"
                              item-text="description"
                              item-value=""
                              :return-object="false"
                              label="Datacenter"
                              placeholder="Select a datacenter or all datacenters"
                              class="pt-8 pl-4"
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
        props: ['updateData'],
        item: null,
        data () {
            return {
                options: {},
                totalItems: 0,
                loading: true,
                headers: [
                    { text: 'Address', value: 'address'},
                    { text: 'Datacenter', value: 'datacenter.description' },
                    { text: 'Actions', value: 'action', sortable: false },
                ],
                racks: [],
                datacenters: [],
                selectedDatacenter: 'All Datacenters',
                searchQuery: {
                    datacenter: '',
                    page: 0,
                    pageSize: 0,
                    isDesc: '',
                    sortBy: '',
                }
            };
        },
        watch: { // This might slow things down if we have a lot of racks to get from the backend !!!
            updateData(val) {
                this.updateRacks() || val;
            },
            options: {
                handler() {
                    this.getDataFromApi()
                        .then(data => {
                            this.racks = data.data;
                            this.totalItems = data.totalCount;
                            this.loading = false;
                        })
                },
                deep: true
            },
        },
        computed: {
            admin() {
                return Auth.isAdmin()
            },
            filteredHeaders() {
                return (this.admin) ? this.headers : this.headers.filter(h => h.text !== "Actions")
            },
        },
        mounted() {
            this.getDataFromApi()
                .then(data => {
                    this.racks = data.data;
                    this.totalItems = data.totalCount;
                    this.loading = false;
                })
        },
        async created () {
            this.initializeDatacenters()
        },
        methods: {
            async initializeDatacenters() {
                this.datacenters = await this.datacenterRepository.list();
                var datacenter = {
                    description: "All Datacenters",
                    name: "All",
                }
                this.datacenters.push(datacenter);
                this.loading = false;

            },
            async getDataFromApi() {
                this.loading = true;
                const { sortBy, sortDesc, page, itemsPerPage } = this.options;

                this.fillQuery(sortBy, sortDesc, page, itemsPerPage);
                /* eslint-disable no-unused-vars, no-console */
                console.log("this is the sorting stuff")
                console.log(this.searchQuery);

                var info = await this.rackRepository.tablelist(this.searchQuery);
                this.racks = info.data;
                return info;
            },
            async fillQuery(sortBy, sortDesc, page, itemsPerPage) {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);
                if (typeof searchDatacenter === 'undefined') {
                    this.searchQuery.datacenter = '';
                } else {
                    this.searchQuery.datacenter = searchDatacenter.id;
                }
                this.searchQuery.page = page;
                this.searchQuery.pageSize = itemsPerPage;
                this.searchQuery.sortBy = this.parseSort(sortBy);
                this.searchQuery.isDesc = this.parseSort(sortDesc);
            },
            parseSort(value) {
                if (typeof value === 'undefined') {
                    return '';
                }
                else if (value.length !== 0) {
                    return value[0];
                }
                return '';
            },
            deleteItem(item) {
                confirm('Are you sure you want to delete this item?') && this.rackRepository.deleteInRange(item.address, item.address, item.datacenterId)
                    .then(async () => {
                        this.datacenterSearch();
                    })
            },
            async datacenterSearch() {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);
                this.racks = await this.rackRepository.list(searchDatacenter.id); 
            },
            async updateRacks() { // This might slow things down if we have a lot of racks to get from the backend !!!
                this.$emit('updated');
                this.datacenterSearch(); // re-call with same datacenter name if racks were added or deleted
            }
        }
    }
</script>