<template>
    <div>
        <v-card flat>
            <v-card-title>System Log</v-card-title>
            <v-container>
                <v-card>
                    <v-data-table :headers=headers
                                  :items="items"
                                  :search="search"
                                  :options.sync="options"
                                  :server-items-length="totalItems"
                                  :expanded.sync="expanded"
                                  :loading="loading"
                                  show-expand
                                  @click:row="expand">
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
                        <template v-slot:expanded-item="{ headers, item }">
                            <td :colspan="headers.length">
                                <v-container>
                                    <v-row>
                                        <v-col v-for="(table, title, index) in item.data" :key="index">
                                            <v-subheader>{{ title }}</v-subheader>
                                            <v-simple-table dense fixed-header>
                                                <template v-slot:default>
                                                    <thead>
                                                        <tr>
                                                            <th>Property</th>
                                                            <th>Value</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr v-for="(value, name) in table" :key="name" class="text-left">
                                                            <td class="font-weight-medium">{{ name }}</td>
                                                            <td v-if="isIdProperty(name, item)"><router-link :to="constructLink(value, name, item)">{{ value }}</router-link></td>
                                                            <td v-else>{{ value }}</td>
                                                        </tr>
                                                    </tbody>
                                                </template>
                                            </v-simple-table>
                                        </v-col>
                                    </v-row>
                                </v-container>
                            </td>
                        </template>
                    </v-data-table>
                </v-card>
            </v-container>

        </v-card>
    </div>
</template>


<script>
    export default {
        components: {
        },
        inject: ['logRepository'],
        data() {
            return {
                options: {},
                totalItems: 0,
                loading: true,
                search: '',
                headers: [
                    { text: 'Event', value: 'event', sortable: false }, // nature of the event (e.g. asset created)
                    { text: 'Date', value: 'created', sortable: false }, // Time stamp
                    {
                        text: 'User',
                        align: 'left',
                        value: 'subjectName', // User
                        sortable: false
                    },
                    // Log entries shall include the initiating
                    // user, the entities involved, the nature of the event, and the time and date
                    // Need to link each log to the appropriate details page 
                    // Would come from data
                ],
                items: [],
                expanded: []
            }
        },
        watch: {
            options: {
                handler() {
                    this.getDataFromApi()
                        .then(data => {
                            this.items = data.data;
                            this.totalItems = data.totalCount;
                            this.parseJsonProperties();
                            this.loading = false;
                        })
                },
                deep: true
            },
        },
        methods: {
            getDataFromApi() {
                this.loading = true;
                const { page, itemsPerPage } = this.options;
                return this.logRepository.list(page, itemsPerPage, this.search);
            },
            expand(value) {
                let index = this.expanded.indexOf(value);
                if (index >= 0) this.expanded.splice(index, 1);
                else this.expanded.push(value);
            },
            getEntityType(name, item) {
                var entityType = name.toLowerCase().replace('id', '');
                if (entityType === '')
                    entityType = item.category.toLowerCase();
                return entityType;
            },
            isIdProperty(name, item) {
                var entityType = this.getEntityType(name, item);
                return name.toLowerCase().endsWith('id') && ['asset', 'model'].includes(entityType);
            },
            constructLink(value, name, item) {
                var entityType = this.getEntityType(name, item);
                return {
                    name: `${entityType}-details`,
                    params: { id: value }
                }
            },
            parseJsonProperties() {
                if (typeof this.items !== 'undefined') {
                    this.items.forEach(log => {
                        if (typeof log === 'undefined')
                            return;
                        if (typeof log.action !== 'undefined')
                            log.action = JSON.parse(log.action);
                        if (typeof log.data !== 'undefined')
                            log.data = JSON.parse(log.data);
                    });
                }
            }
        }
    }
</script>
