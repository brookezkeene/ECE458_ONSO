<template>
    <div>
        <v-card flat>
            <v-card-title>System Log</v-card-title>
            <v-container>
                <v-card>
                    <v-data-table :headers="filteredHeaders"
                                  :items="logEntries"
                                  :search="search"
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

                loading: true,
                search: '',
                headers: [
                    {
                        text: 'Subject Name',
                        align: 'left',
                        value: 'subjectName'
                    },
                    { text: 'Action', value: 'action'},
                    { text: 'Data', value: 'data'}, 
                    { text: 'Created', value: 'created'},
                    { text: 'Event', value: 'event' },
                ],
                logEntries: [],
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
                this.log = await this.logRepository.list();
                this.loading = false;
            },
        }
    }
</script>
