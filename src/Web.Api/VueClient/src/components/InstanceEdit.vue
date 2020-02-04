<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">Edit</span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-row>
                        <v-col cols="12" sm="6" md="4">
                            <v-autocomplete v-model="editedItem.model.vendor"
                                            :items="models"
                                            item-text="vendor"
                                            label="Vendor">
                            </v-autocomplete>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-autocomplete v-model="editedItem.model.modelNumber"
                                            :items="models"
                                            item-text="modelNumber"
                                            label="Model Number">
                            </v-autocomplete>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-combobox v-model="editedItem.hostname"
                                        :items="instances"
                                        item-text="hostname"
                                        label="Host Name">
                            </v-combobox>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="editedItem.rack" label="Rack Number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="editedItem.rackPosition" label="Rack Position"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="editedItem.owner.username" label="Owner User Name"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="editedItem.owner.firstName" label="Owner First Name"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="editedItem.owner.lastName" label="Owner Last Name"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="editedItem.owner.email" label="Owner email"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="editedItem.comment" label="Comment"></v-text-field>
                        </v-col>
                    </v-row>
                </v-container>
            </v-card-text>
        </v-card>
    </div>
</template>

<script>
    export default {
        name: 'instance-edit',
        inject: ['instanceRepository','modelRepository'],

        item: null,
        props: {
            editedItem: Object,
            instances: Array,
            models: Array,
        },
        data() {
            return {
                loading: false,
            };
        },
        editedIndex: -1,
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

            },
            hostname: '',
            rack: '',
            owner: {
                id: '',
                username: '',
                displayname: '',
                email: '',
            },
            rackPosition: '',
            comment: ''
        },
    }
</script>
