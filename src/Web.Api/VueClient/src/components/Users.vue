<template>
    <v-card flat>
        <v-card-title>Users</v-card-title>
        <v-container>
        <v-card>
            <v-data-table :headers="filteredHeaders"
                          :items="users"
                          :search="search"
                          class="pa-10"
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

                        <v-btn v-if="admin" color="primary" dark class="mb-2" @click="openCreateUser">Add User</v-btn>

                    </v-toolbar>

                    <div class="text-center">
                        <v-dialog v-model="permissionsDialog" width="35%">
                            <v-card>
                                <v-card-title>
                                    Edit Permission Level for {{editedItem.username}}
                                </v-card-title>
                                <v-card-text>
                                    <v-container fluid>
                                            <v-checkbox label="Basic"
                                                        v-model="editedRoles"
                                                        disabled
                                                        value="basic"></v-checkbox>
                                            <v-checkbox label="Administrator" 
                                                        v-model="editedRoles"
                                                        value="admin"></v-checkbox>
                                    </v-container>
                                </v-card-text>
                                <v-card-actions>
                                    <v-spacer></v-spacer>
                                    <v-btn color="primary" text @click="closeDialog">Close</v-btn>
                                    <v-btn color="primary" text @click="savePermissions">Save</v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-dialog>
                    </div>
                </template>

                <template v-slot:item.role="{ item }">
                    <v-row class="pl-3">
                        {{item.role}}
                    </v-row>
                </template>

                <template v-if="admin" v-slot:item.action="{ item }">
                    <v-row class="pl-5">
                        <v-icon v-if="showActionsForUser(item)"
                                medium
                                class="mr-2"
                                @click="editPermissions(item)">mdi-pencil</v-icon>
                        <v-icon v-if="showActionsForUser(item)"
                                medium
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
        editedRoles: [],
        permissionsDialog: false,
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

    async created() {
        this.initialize()
    },

    methods: {
        showActionsForUser(item) {
            return !['admin', Auth.username()].includes(item.username);
        },
        async initialize() {
            this.loading = true
            this.users = await this.userRepository.list();
            this.loading = false;
        },
        openCreateUser() {
            this.$router.push({ name: 'users-create' });
        },
        deleteItem(item) {
            confirm('Are you sure you want to delete this user?') && this.userRepository.delete(item.id)
                .then(async () => {
                    await this.initialize();
                })
        },
        editPermissions(item) {
            this.editedItem = item;
            this.userRepository.findRole(item.id)
                .then((roles) => {
                    this.editedRoles = roles;
                    this.permissionsDialog = true;
                })
        },
        savePermissions() {
            this.userRepository.updateUserRoles(this.editedItem.id, { roles: this.editedRoles } )
                .then(async () => {
                    this.closeDialog();
                    await this.initialize();
                })
        },
        closeDialog() {
            this.permissionsDialog = false;
        }
    },
  }
</script>