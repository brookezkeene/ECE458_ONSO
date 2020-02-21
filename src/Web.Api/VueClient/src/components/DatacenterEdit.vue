<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">{{formTitle}}</span>
            </v-card-title>

            <v-card-text>

                <v-container fluid class="justify-center">
                    <v-row>
                        <v-col>
                            <v-text-field v-model="newItem.Name" label="Datacenter Name" counter="50"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="newItem.Description" label="Datacenter Abbreviation" counter="6"></v-text-field>
                        </v-col>
                    </v-row>

                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn text @click="close">Cancel</v-btn>
                        <v-btn color="primary" text @click="save">Save</v-btn>
                    </v-card-actions>'

                </v-container>
            </v-card-text>
        </v-card>
    </div>
</template>

<script>export default {
        name: 'datacenter-edit',
        inject: ['datacenterRepository'],
        props: {
            id: String,
        },
        data() {
            return {
                datacenters: [],
                loading: false,
                newItem: {
                    Name: '',
                    Description: '',
                    HasNetworkManagedPower: true
                },
            };
        },
        editedIndex: -1,

        async created() {
            this.datacenters = await this.datacenterRepository.list();

            this.newItem = typeof this.id === 'undefined'
                ? this.newItem
                : this.datacenters.find(o => o.id === this.id);

        },

        computed: {
            formTitle() {
                return typeof this.id === 'undefined' ? 'New Datacenter' : 'Edit Datacenter'
            },
        },
        methods: {
            save() {
                this.datacenterRepository.create(this.newItem);
                this.close()
            },
            close() {
                setTimeout(() => {
                    this.$router.push({ name: 'racks' })
                }, 300)
            },
        }
    }</script>