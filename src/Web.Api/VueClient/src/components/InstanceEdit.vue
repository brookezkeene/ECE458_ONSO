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
                                            persistent-hint
                                            return-object>
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
                                            return-object>
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
            editedItem: Object,
            isNew: Boolean,
        },
        data() {
            return {
                models: [],
                users: [],
                loading: false,
                newOwner: "",
            };
        },

        async created() {
            this.models = await this.modelRepository.list();
            this.users = await this.userRepository.list();

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
                    if (newUser.username === user.username) {
                        item.owner = newUser;
                    }
                }
                return item;
            },
            updateModel(newModel, item) {

                for (const model of this.models) {
                /*eslint-disable*/
                    console.log(newModel.vendorModelNo)

                    let val = newModel.vendorModelNo.split(" ")

                    if (val[0] === model.vendor && val[1] === model.modelNumber) {
                        item.model = model;
                    }
                }
                return item;
            }
        }
    }
</script>
