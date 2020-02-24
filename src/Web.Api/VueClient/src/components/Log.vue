<template>
    <div>
        <v-card flat>
            <v-card-title>System Log</v-card-title>
            <v-container>
                <v-card>
                    <v-data-table :headers=headers
                                  :items="items"
                                  :search="search"
                                  v-model="page" :length="pageCount"
                                  multi-sort
                                  @click:row="showDetails">
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
                page: 1,
                pageCount: 0,
                loading: true,
                search: '',
                headers: [
                    {
                        text: 'Subject Name',
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
            showDetails() {

            }
        }
    }
</script>
