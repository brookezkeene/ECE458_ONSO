<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">{{formTitle}}</span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-row>
                        <v-col cols="12" sm="6" md="4">
                            <v-autocomplete v-model="editedItem.modelId"
                                            :items="models"
                                            item-text="vendorModelNo"
                                            item-value="id"
                                            label="Model">
                            </v-autocomplete>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="editedItem.hostname"
                                          label="Host Name"
                                          counter="255">
                            </v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-autocomplete v-model="editedItem.datacenter"
                                            label="Data Center"
                                            :items="datacenters"
                                            item-text="name"
                                            item-value="id">
                            </v-autocomplete>
                        </v-col>
                        <!-- Will need to update to show only racks from the selected datacenter -->
                        <v-col cols="12" sm="6" md="4">
                            <v-autocomplete v-model="editedItem.rackId"
                                            label="Rack Number"
                                            :items="racks"
                                            item-text="address"
                                            item-value="id">
                            </v-autocomplete>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="editedItem.rackPosition" label="Rack Position" type="number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-autocomplete v-model=editedItem.ownerId
                                            :items="users"
                                            item-text="username"
                                            item-value="id"
                                            label="Owner User Name"
                                            persistent-hint>
                            </v-autocomplete>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="editedItem.comment" label="Comment"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-container>
                                <v-title>Power Port Connections</v-title>
                                <v-for editedItem.js></v-for>
                            </v-container>
                        </v-col>
                    </v-row>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn text @click="close">Cancel</v-btn>
                        <v-btn color="primary" text @click="save">Save</v-btn>
                    </v-card-actions>'
                </v-container>
            </v-card-text>
            <template>
                <!-- dialog to set power port connections -->
                <div class="text-center">
                    <v-dialog v-model="namesDialog" width="400">
                        <v-card class="overflow-y-auto" max-height="500px">
                            <v-card-title>
                                Edit Power Port Connections
                            </v-card-title>
                            <v-card-text>
                                <v-container fluid>
                                    <div v-for="(n, index) in this.powerPortConnections" :key="n">
                                        <v-text-field v-model="networkPortNames[index]"
                                                      label="Network Port"
                                                      placeholder="port name"
                                                      :rules="[rules.networkPortRules]"
                                                      :value="n"></v-text-field>
                                    </div>
                                </v-container>
                            </v-card-text>
                            <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn color="primary" text @click="saveNames">Save</v-btn>
                                <v-btn color="primary" text @click="closeNamesDialog">Close</v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-dialog>
                </div>
            </template>
        </v-card>
    </div>
</template>

<script>
    export default {
        name: 'asset-edit',
        inject: ['assetRepository', 'modelRepository', 'userRepository', 'rackRepository', 'datacenterRepository'],
        props: {
            id: String,
        },
        data() {
            return {
                models: [],
                users: [],
                assets: [],
                racks: [],
                datacenters: [],
                loading: false,
                ownerId: '',
                editedItem: {
                    datacenter: '',
                    modelId: '',
                    hostname: '',
                    rackId: '',
                    rackPosition: '',
                    ownerId: '',
                    comment: '',
                },
                rackNumber: 0,
                powerPortConnections: [],
                namesDialog: false,
            }
        },

        async created() {
            this.models = await this.modelRepository.list();
            this.users = await this.userRepository.list();
            this.assets = await this.assetRepository.list();
            this.racks = await this.rackRepository.list();
            this.datacenters = await this.datacenterRepository.list();

            const existingItem = this.assets.find(o => o.id == this.id);
            if (typeof existingItem !== 'undefined') {
                this.editedItem = Object.assign({}, existingItem);
            }

            for (const model of this.models) {
                model.vendorModelNo = model.vendor + " " + model.modelNumber;
            }

        /*eslint-disable*/
            console.log(this.editedItem);
        },

        computed: {
            formTitle() {
                 return typeof this.id === 'undefined' ? 'New Item' : 'Edit Item'
            }
        },
        methods: {
            save() {

                if (typeof this.id !== 'undefined') {
                    console.log(this.editedItem);
                    this.assetRepository.update(this.editedItem).then(this.close());
                } else {
                    console.log(this.editedItem)
                    this.assetRepository.create(this.editedItem).then(this.close());
                }
            },
            close() {
                this.$router.push({ name: 'assets' })
            },
            openNamesDialog() {
                this.namesDialog = true;
            },
            saveNames() {
                /* eslint-disable no-unused-vars, no-console */
                console.log(this.networkPortNames);
                var i;
                for (i = 0; i < this.networkPortNames.length; i++) {
                    if (this.networkPortNames[i] === null) {
                        var portObjDefault = Object.assign({}, { name: (i+1).toString(), number: i+1 })
                        this.networkPorts[i] = portObjDefault;
                    } else {
                        var portObj = Object.assign({}, { name: this.networkPortNames[i], number: i+1 })
                        this.networkPorts[i] = portObj;
                    }
                }
                console.log(this.networkPorts);
                this.namesDialog = false;
            },
            closeNamesDialog() {
                this.namesDialog = false;
            },
        }
    }
</script>
