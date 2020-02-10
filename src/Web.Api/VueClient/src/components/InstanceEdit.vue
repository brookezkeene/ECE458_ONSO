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
                            <v-autocomplete v-model="editedItem.model.vendorModelNo"
                                            :items="models"
                                            item-text="vendorModelNo"
                                            item-value="vendorModelNo"
                                            label="Model"
                                            :return-object="false">
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
                            <v-text-field v-model="editedItem.rackPosition" label="Rack Position" type="number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-autocomplete v-model="newOwner"
                                            :items="users"
                                            item-text="username"
                                            item-value="username"
                                            label="Owner User Name"
                                            persistent-hint
                                            :return-object="false">
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
        props: ['id','isNew'],
        data() {
            return {
                models: [],
                users: [],
                instances: [],
                loading: false,
                newOwner: "",
                item: {
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
                        rack: '',
                        owner: {
                            id: '',
                            username: '',
                            firstName: '',
                            lastName: '',
                            email: '',
                        },
                        rackPosition: '',
                        comment: ''
                },
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
                  rackPosition:'',
                  comment: ''
                }
            }
        },

        async created() {
            this.models = await this.modelRepository.list();
            this.users = await this.userRepository.list();
            this.instances = await this.instanceRepository.list();
            if (this.instances.find(o => o.id === this.id) != undefined) {
                this.editedItem = await this.instances.find(o => o.id === this.id);
            } else {
                this.editemItem = Object.assign({}, this.item);
            }

            for (const model of this.models) {
                model.vendorModelNo = model.vendor + " " + model.modelNumber;
            }
        },

        computed: {
            formTitle() {
                return this.isNew ? 'New Item' : 'Edit Item'
            },
        },
        methods: {
            save() {

                this.updateOwner(this.newOwner, this.editedItem);
                this.updateModel(this.editedItem.model.vendorModelNo, this.editedItem);

                if (!this.isNew) {
                    this.instanceRepository.update(this.editedItem);
                } else {
                    this.instanceRepository.create(this.editedItem);
                }
                this.close()
            },
            close() {
                setTimeout(() => {
                    this.$router.push({ name: 'instances'})
                }, 300)
            },
            updateOwner(newUser, item) {

                for (const user of this.users) {
                                    /*eslint-disable*/
                    console.log(newUser);
                    if (newUser === user.username) {
                        console.log(newUser);
                        item.owner.username = newUser;
                        item.owner.firstName = user.firstName;
                        item.owner.lastName = user.lastName;
                        item.owner.email = user.email;
                    }
                }
                return item;
            },
            updateModel(newModel, item) {

                for (const model of this.models) {
                /*eslint-disable*/
                    console.log(newModel)

                    /*Potential Bug if have a model, modelNo combo with more than one space*/
                    let val = newModel.split(" ")

                    if (val[0] === model.vendor && val[1] === model.modelNumber) {
                        item.model = model;
                    }
                }
                return item;
            }
        }
    }
</script>
