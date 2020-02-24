<template>
    <div>
        <v-card flat>
            <v-card-title>System Log</v-card-title>
            <v-container>
                <v-card>
                    <v-data-table :headers=headers
                                  :items="items"
                                  :search="search"
                                  :pagination.sync="pagination"
                                  :total-items="totalItems"
\                                  multi-sort
                                  @click:row="routeToDetails">
                        <template v-slot:top v-slot:item.action="{ item }">

                            <v-toolbar flat color="white">
                                <v-autocomplete prepend-inner-icon="mdi-magnify"
                                                :search-input.sync="search"
                                                cache-items
                                                flat
                                                hide-no-data
                                                hide-details
                                                label="Search"
                                                single-line
                                                solo-inverted></v-autocomplete>
                                <v-spacer></v-spacer>
                            </v-toolbar>
                        </template>

                    </v-data-table>
                </v-card>
            </v-container>

        </v-card>
    </div>
</template>


<script>
    import Auth from "../auth"

    export default {
        components: {
        },
        inject: ['logRepository'],
        data() {
            return {
                pagination: {},
                totalItems: 0,
                loading: true,
                search: '',
                headers: [
                    {
                        text: 'User',
                        align: 'left',
                        value: 'subjectName', // User
                        sortable: false
                    },
                    // Log entries shall include the initiating
                    // user, the entities involved, the nature of the event, and the time and date.
                    { text: 'Event', value: 'event',sortable: false }, // nature of the event (e.g. asset created)
                    { text: 'Category', value: 'category', sortable: false},  // Which entity was involved
                    { text: 'Created', value: 'created', sortable: false }, // Time stamp
                    { text: 'Data', value: 'data', sortable: false },

                    // Need to link each log to the appropriate details page 
                    // Would come from data
                ],
                logEntries: [],
                items: [],
                logItem: {
                    subjectName: '',
                    action: '',
                    data: '',
                    created: '',
                    event:'',
                },
            }
        },
        computed: {
            admin() {
                return Auth.isAdmin()
            },
        },
        async created() {
            this.initialize()
        },
        watch: {
          pagination: {
            handler () {
              this.logRepository.list()
                .then(data => {
                  this.items = data.data
                  this.totalItems = data.totalCount
                })
            },
          },
        methods: {
            async initialize() {
                this.logEntries = await this.logRepository.list();
                this.page = this.logEntries.currentPage;
                this.pageCount = this.logEntries.totalCount;
                this.items = this.logEntries.data;

                /*eslint-disable*/
                console.log(this.logEntries);
                this.loading = false;
            },
            routeToDetails(item) {
                console.log(item);
                var data = JSON.parse(item.data);
                console.log(data);
                console.log(data.Id);
                if (item.category = 'Asset') {
                    this.routeToAssetDetails(data.Id)
                } else if (item.category = 'Model') {
                    this.routeToModelDetails(data.Id)
                } else if (item.category = 'User') {
                    this.routeToUserDetails(data.Id)
                }
            },
            routeToModelDetails(id) {
                this.$router.push({ name: 'model-details', params: { id: id } })
            },
            routeToAssetDetails(id) {
                this.$router.push({ name: 'asset-details', params: { id: id } })
            },
            routeToUserDetails(id) {
                this.$router.push({ name: 'user-details', params: { id: id } })
            }
        }
    }
</script>
