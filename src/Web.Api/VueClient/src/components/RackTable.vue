<template>
    <v-card>
        <v-data-table
          :headers="filteredHeaders"
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
        inject: ['rackRepository'],
        item: null,
        props: {
            editedItem:Object
        },
        data () {
            return {
                admin: false,
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
        computed: {
            isAdmin() {
                return Auth.isAdmin()
            },
            filteredHeaders() {
                return (this.isAdmin) ? this.headers : this.headers.filter(h => h.text !== "Actions")
            },
        },
        async created () {
            this.initialize()
        },
        methods: {
            async initialize () {
                this.racks = await this.rackRepository.list();
                this.loading = false;
            },
            deleteItem (item) {
                confirm('Are you sure you want to delete this item?') && this.rackRepository.deleteInRange(item.address, item.address)
            },
        }
    }
</script>