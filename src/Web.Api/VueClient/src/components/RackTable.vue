﻿<template>
    <v-card>
        <v-data-table
          :headers="headers"
          :items="racks"
          >
            <template v-slot:top>

            <v-toolbar flat color="white">
                <v-toolbar-title>Existing Racks</v-toolbar-title>
                <v-divider
                class="mx-4"
                inset
                vertical
                ></v-divider>
            </v-toolbar>

            </template>

            <template v-slot:item.action="{ item }">
            <v-icon
                small
                @click="deleteItem(item)"
            >
                delete
            </v-icon>
            </template>

            <template v-slot:no-data>
                <v-btn color="primary" @click="initialize">Reset</v-btn>
            </template>
        </v-data-table>
    </v-card>
</template>

<script>
    export default {
        name: 'rack-table',
        inject: ['rackRepository'],
        item: null,
        props: {
            editedItem:Object
        },
        data () {
            return {
                loading: true,
                headers: [
                    { text: 'Address', value: 'address'},
                    { text: 'Row Letter', value: 'position.row' },
                    { text: 'Rack Number', value: 'position.column' },
                    { text: 'Actions', value: 'action', sortable: false },
                ],
                racks: [],
            };
        },
        async created () {
            this.initialize()
        },
        methods: {
            async initialize () {
                this.racks = await this.rackRepository.list();
                this.firstRack = await this.rackRepository.find(1);
                this.loading = false;
            },
            deleteItem (item) {
                const index = this.racks.indexOf(item)
                confirm('Are you sure you want to delete this item?') && this.racks.splice(index, 1)
            },
        }
    }
</script>