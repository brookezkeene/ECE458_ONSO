<template>
    <v-card flat>
        <v-card-title>Users</v-card-title>
        <v-container>
        <v-card>
            <v-data-table :headers="filteredHeaders"
                          :items="users"
                          :search="search"
                          multi-sort>
                <template v-slot:top>
                    <v-toolbar flat color="white">
                        <v-autocomplete :loading="loading"
                                        :items="users"
                                        :search-input.sync="search"
                                        prepend-inner-icon="mdi-magnify"
                                        cache-items
                                        flat
                                        hide-no-data
                                        hide-details
                                        item-text="username"
                                        label="Search"
                                        single-line
                                        solo-inverted></v-autocomplete>
                        <v-spacer></v-spacer>

                        <v-btn v-if="admin" color="primary" dark class="mb-2" v-on="on" @click="openCreateUser">Add User</v-btn>

                    </v-toolbar>

                    <div class="text-center">
                        <v-dialog v-model="permissionsDialog" width="250">
                            <v-card>
                                <v-card-title>
                                    Edit Permission Level for {{editedItem.username}}
                                </v-card-title>
                                <v-card-text>
                                    <v-container fluid>
                                        <p>{{ permissionsRadio }}</p>
                                        <v-radio-group v-model="permissionsRadio" :mandatory="false"> <!--should be editedItem.permission-->
                                            <v-radio label="Regular" value="regular"></v-radio>
                                            <v-radio label="Administrator" value="admin"></v-radio>
                                        </v-radio-group>
                                    </v-container>
                                </v-card-text>
                                <v-card-actions>
                                    <v-spacer></v-spacer>
                                    <v-btn color="primary" text @click="savePermissions(item)">Save</v-btn>
                                    <v-btn color="primary" text @click="closeDialog">Close</v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-dialog>
                    </div>
                </template>

                <template v-slot:item.permission="{ item }">
                    <v-row class="pl-3">
                        {{item.username}}
                        <v-icon v-if="admin"
                                medium
                                class="mr-2"
                                @click="editPermissions(item)">mdi-pencil</v-icon>
                    </v-row>
                </template>

                <template v-if="admin" v-slot:item.action="{ item }">
                    <v-row class="pl-5">
                        <v-icon medium
                                class="mr-2"
                                @click="deleteItem(item)">mdi-delete</v-icon>
                    </v-row>
                </template>
            </v-data-table>
        </v-card>
        </v-container>
    </v-card>
</template>

<script>
import Auth from "../auth"

  export default {
    components: {
    },
    inject: ['userRepository'],
    data () {
      return {
        dialog: false,
        loading: true,
        search: '',
        headers: [
            { text: 'First Name', value: 'firstName' },
            { text: 'Last Name', value: 'lastName' },
            { text: 'Username', value: 'username' },
            { text: 'Email', value: 'email' },
            { text: 'Permission Level', value: 'permission' },
            { text: 'Actions', value: 'action', sortable: false },
        ],
        users: [],
        editedIndex: -1,
        editedItem: {
            firstName: '',
            lastName: '',
            email: '',
            username: '',
            password: ''
        },
        defaultItem: {
            firstName: '',
            lastName: '',
            email: '',
            username: '',
            password: ''
        }, 
        permissionsDialog: false,
        permissionsRadio: '',
      }
    },
    computed: {
        admin() {
            return Auth.isAdmin()
        },
        filteredHeaders() {
                return (this.admin) ? this.headers : this.headers.filter(h => h.text !== "Actions")
            },
    },

    watch: {
      dialog (val) {
        val || this.close()
      },
      permissionsDialog (val) {
        val || this.closeDialog()
      },

    },

    async created() {
        this.initialize()
    },

    methods: {
        async initialize() {
            this.users = await this.userRepository.list();
            this.loading = false;
        },
        openCreateUser() {
            this.$router.push({ name: 'users-create' });
        },
        deleteItem(item) {
            confirm('Are you sure you want to delete this user?') && this.userRepository.delete(item)
                .then(async () => {
                    await this.initialize();
                })
        },
        editPermissions(item) {
            this.editedItem = Object.assign({}, item);
            this.permissionsRadio = item.username; // change to permission prop - item.permission
            this.permissionsDialog = true;
        },
        savePermissions(item) {
            // save new permissionsRadio value as user item's permission
            // item.permission = this.permissionsRadio
            this.userRepository.update(item);
        },
        closeDialog() {
            this.permissionsDialog = false;
        }
    },
  }
</script>