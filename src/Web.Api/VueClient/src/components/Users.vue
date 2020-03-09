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
                                            <v-row v-for="(role, index) in editingRoles" :key="index">
                                                <v-checkbox :label="role.label"
                                                            :v-model="role.value"
                                                            :value="role.value"></v-checkbox>
                                                <v-tooltip right>
                                                    <template v-slot:activator="{ on }">
                                                        <v-icon class="pl-2" color="primary" v-on="on">mdi-information-outline</v-icon>
                                                    </template>
                                                    <span>{{role.description}}</span>
                                                </v-tooltip>
                                            </v-row>
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
        <v-snackbar v-model="updateSnackbar.show"
                    :bottom=true
                    class="black--text"
                    :color="updateSnackbar.color"
                    :timeout=5000>
            {{updateSnackbar.message}}
            <v-btn dark
                   class="black--text"
                   text
                   @click="updateSnackbar.show = false">
                Close
            </v-btn>
        </v-snackbar>
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
          updateSnackbar: {
                    show: false,
                    message: '',
                    color: ''
                },
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
        editingRoles() {
            // TODO: update if user has existing permissions
            var roles = [
                { name: 'model', label: 'Model Management Permission', description: 'Allows creation, modification, and deletion of models.', value: false }, 
                { name: 'asset', label: 'Asset Management Permission', description: 'Allows creation, modification, decommissioning, and deletion of assets. May be conferred globally or per-datacenter.', value: false },
                { name: 'power', label: 'Power Permission', description: 'Allows power control of assets for users that are not the explicit owners of the asset in question', value: false },
                { name: 'audit', label: 'Audit Permission', description: 'Allows reading of the audit log.', value: false },
                { name: 'admin', label: 'Administrator Permission', description: 'Inherits all of the abilities of the other permissions. Can also confer or revoke permissions onto users.', value: false },
            ]

            if (roles[4].value == true) {
                for (var i = 0; i < 4; i++) {
                    roles[i].value == true;
                }
            }

            return roles
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
        async deleteItem(item) {
            var split = (item.email).split("@");

            if (split[1].toUpperCase() === 'DUKE.EDU') {
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'red lighten-4';
                this.updateSnackbar.message = 'Cannot delete duke students.';
                return;
            }
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
            /* eslint-disable no-unused-vars, no-console */
            console.log(this.editingRoles);
            this.editedRoles = [];
            for (var i = 0; i < this.editingRoles.length; i++) {
                if (this.editingRoles[i].value) {
                    this.editedRoles.push(this.editingRoles[i].name)
                }
            }
            console.log(this.editedRoles);
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