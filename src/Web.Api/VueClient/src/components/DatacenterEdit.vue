<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">{{formTitle}}</span>
            </v-card-title>

            <v-container>
                <v-form v-model="valid">
                    <v-row>
                        <v-col>
                            <v-text-field v-if="type==='datacenters'"
                                          v-model="newItem.description"
                                          label="Datacenter Name"
                                          :rules="[rules.nameRules]"
                                          counter="50"></v-text-field>
                            <v-text-field v-else
                                          v-model="newItem.description"
                                          label="Offline Storage Name"
                                          :rules="[rules.nameRules]"
                                          counter="50"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-if="type==='datacenters'" v-model="newItem.name" label="Datacenter Abbreviation" counter="6"></v-text-field>
                            <v-text-field v-else v-model="newItem.name" label="Offline Storage Abbreviation" counter="6"></v-text-field>
                        </v-col>
                    </v-row>

                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn @click="close">Cancel</v-btn>
                        <v-btn color="primary" :disabled="!valid" @click="save">Save</v-btn>
                    </v-card-actions>'
                </v-form>
            </v-container>
        </v-card>
    </div>
</template>

<script>export default {
        name: 'datacenter-edit',
        inject: ['datacenterRepository'],
        props: {
            id: String,
            type: String,
        },
        data() {
            return {
                datacenters: [],
                loading: false,
                newItem: {
                    name: '',
                    description: '',
                    HasNetworkManagedPower: false
                },
                rules: {
                    nameRules: v => /^(?=\s*\S).*$/.test(v) || 'Name is required',
                },
                valid: true
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
                if (this.type === 'datacenters') {
                    return typeof this.id === 'undefined' ? 'New Datacenter' : 'Edit Datacenter'
                } else {
                    return typeof this.id === 'undefined' ? 'New Offline Storage Site' : 'Edit Offline Storage Site'
                }
            },
        },
        methods: {
            save() {
                if (this.newItem.name.toLowerCase() === "rtp1") {
                    this.newItem.HasNetworkManagedPower = true;
                }

                if (typeof this.id === 'undefined') {
                    this.datacenterRepository.create(this.newItem);
                } else {
                    this.datacenterRepository.update(this.newItem);
                }
                this.close()
            },
            close() {
                setTimeout(() => {
                    this.$router.push({ name: 'sites', params: { type: this.type } })
                }, 300)
            },
        }
    }</script>