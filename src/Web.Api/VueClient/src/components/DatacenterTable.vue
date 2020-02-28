﻿<template>
    <v-card>
        <v-data-table :headers="filteredHeaders"
                      :items="datacenters"
                      :search="search"
                      class="pa-10"
                      multi-sort>
            <template v-slot:top>

                <v-toolbar flat color="white">
                    <v-toolbar-title>Datacenters</v-toolbar-title>

                    <v-spacer></v-spacer>

                    <v-btn v-if="admin" color="primary" dark class="mb-2" @click="openCreate">Add Datacenter</v-btn>
                </v-toolbar>

                <v-toolbar flat>
                    <v-autocomplete :loading="loading"
                                    :items="datacenters"
                                    :search-input.sync="search"
                                    prepend-inner-icon="mdi-magnify"
                                    cache-items
                                    flat
                                    hide-no-data
                                    hide-details
                                    item-text="name"
                                    label="Search"
                                    single-line
                                    solo-inverted></v-autocomplete>
                    <v-spacer></v-spacer>
                    <v-spacer></v-spacer>
                </v-toolbar>

            </template>

            <template v-slot:item.action="{ item }">
                <v-row class="pl-2">
                    <v-icon medium
                            @click="editItem(item)">mdi-pencil</v-icon>
                    <v-icon medium
                            @click="deleteItem(item)">mdi-delete</v-icon>
                </v-row>
            </template>

            <template v-slot:no-data>
                <v-btn color="primary" @click="initialize">Refresh</v-btn>
            </template>
        </v-data-table>
        <v-snackbar v-model="updateSnackbar.show"
                    :bottom=true
                    class="black--text"
                    :color="updateSnackbar.color"
                    :timeout=5000>
            {{updateSnackbar.message}}
            <v-btn dark
                   class="black--text"
                   text
                   @click="updateSnackbar.show = false">
                Close
            </v-btn>
        </v-snackbar>
    </v-card>
</template>

<script>
    import Auth from "../auth"

    export default {
        name: 'datacenter-table',
        inject: ['datacenterRepository', 'rackRepository'],
        item: null,
        data () {
            return {
                updateSnackbar: {
                    show: false,
                    message: '',
                    color: ''
                },
                loading: true,
                headers: [
                    { text: 'Name', value: 'description'},
                    { text: 'Abbreviation', value: 'name' },
                    { text: 'Actions', value: 'action', sortable: false },
                ],
                datacenters: [],
                editedItem: {
                    Name: '',
                    Description: ''
                },
                search: '',
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
                this.datacenters = await this.datacenterRepository.list();
                this.loading = false;
            },
            openCreate() {
                this.$router.push({ name: 'datacenter-create' })
            },
            editItem(item) {
                this.$router.push({ name: 'datacenter-edit', params: { id: item.id } })
            },
            async deleteItem(item) {
                var ifRacks = await this.rackRepository.list(item.id)
                if (ifRacks != null && ifRacks.length > 0) {

                    this.updateSnackbar.show = true;
                    this.updateSnackbar.color = 'red lighten-4';
                    this.updateSnackbar.message = 'Racks of this datacenter exist, cannot delete';
                    return;
                }
                confirm('Are you sure you want to delete this item?') && this.datacenterRepository.delete(item)
                    .then(async () => {
                        await this.initialize();
                    })
            },
        }
    }</script>