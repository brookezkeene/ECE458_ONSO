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
                            <v-autocomplete v-model="editedItem.dataCenter"
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

<script>
    export default {
        name: 'instance-edit',
        inject: ['instanceRepository', 'modelRepository', 'userRepository', 'rackRepository', 'datacenterRepository'],
        props: {
            id: String,
        },
        data() {
            return {
                models: [],
                users: [],
                instances: [],
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
            }
        },

        async created() {
            this.models = await this.modelRepository.list();
            this.users = await this.userRepository.list();
            this.instances = await this.instanceRepository.list();
            this.racks = await this.rackRepository.list();
            this.datacenters = await this.datacenterRepository.list();

            /*eslint-disable*/
            console.log(this.racks);

            const existingItem = this.instances.find(o => o.id == this.id);
            if (typeof existingItem !== 'undefined') {
                this.editedItem = Object.assign({}, existingItem);
/*                this.getOwnerId(existingItem);
*/            }

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
            getOwnerId(instance) {
                if (instance.owner == null) {
                    this.ownerId = '';
                } else {
                    this.ownerId = instance.owner.id;
                }
            },
            save() {

                if (typeof this.id !== 'undefined') {
                    console.log(this.editedItem);
                    this.instanceRepository.update(this.editedItem).then(this.close());
                } else {
                    console.log(this.editedItem)
                    this.instanceRepository.create(this.editedItem).then(this.close());
                }
            },
            close() {
                this.$router.push({ name: 'instances' })
            },
        }
    }
</script>
