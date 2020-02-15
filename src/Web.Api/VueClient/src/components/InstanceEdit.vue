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
                            <v-autocomplete v-model="editedItem.model.id"
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
                            <v-text-field v-model="editedItem.rack" label="Rack Number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="editedItem.rackPosition" label="Rack Position" type="number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-autocomplete v-model=ownerId
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
        inject: ['instanceRepository', 'modelRepository', 'userRepository'],
        props: {
            id: String,
        },
        data() {
            return {
                ownerId: '',

                models: [],
                users: [],
                instances: [],
                loading: false,
                editedItem: {
                  model: {
                    id: '',
                    vendor: '',
                    modelNumber: '',
                    height: 0,
                    displayColor: '',
                    ethernetPorts: 0,
                    powerPorts: 0,
                    cpu: '',
                    memory: '',
                        storage: '',
                    vendorModelNo: ''
                  },
                  hostname: '',
                  rack:'',
                  owner:{
                    id:'',
                    username:'',
                    firstName:'',
                    lastName:'',
                    email:'',
                  },
                  rackPosition:0,
                  comment: ''
                }
            }
        },

        async created() {
            this.models = await this.modelRepository.list();
            this.users = await this.userRepository.list();
            this.instances = await this.instanceRepository.list();

            const existingItem = this.instances.find(o => o.id == this.id);
            if (typeof existingItem !== 'undefined') {
                this.editedItem = Object.assign({}, existingItem);
                this.getOwnerId(existingItem);
            }

            for (const model of this.models) {
                model.vendorModelNo = model.vendor + " " + model.modelNumber;
            }
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

                this.updateOwner();
                this.updateModel();

                if (typeof this.id !== 'undefined') {
                    this.instanceRepository.update(this.editedItem).then(this.close);
                } else {
                    this.instanceRepository.create(this.editedItem).then(this.close);
                }
                this.close();
            },
            close() {
                this.$router.push({ name: 'instances'})
            },
            updateOwner() {

                if (this.ownerId != null) {
                    const userId = this.ownerId;
                    this.editedItem.owner = this.users.find(o => o.id === userId);
                }
            },
            updateModel() {
                const modelId = this.editedItem.model.id;
                this.editedItem.model = this.models.find(o => o.id == modelId);
            }
        }
    }
</script>
