<template>
    <v-card flat>
        <v-card-title>Users</v-card-title>
        <v-container>
        <v-card>
            <v-data-table
                :headers="headers"
                :items="users"
                :search="search"
                multi-sort
            >
                <template v-slot:top>
                <v-toolbar flat color="white">
                    <v-autocomplete
                      v-model="select"
                      :loading="loading"
                      :items="users"
                      :search-input.sync="search"
                      prepend-inner-icon="mdi-magnify"
                      cache-items
                      flat
                      hide-no-data
                      hide-details
                      item-text="displayName"
                      label="Search"
                      single-line
                      solo-inverted
                    ></v-autocomplete>
                    <v-spacer></v-spacer>
                    <v-dialog v-model="dialog" max-width="500px">
                        <template v-slot:activator="{ on }">
                            <v-btn v-if="admin" color="primary" dark class="mb-2" v-on="on">Add User</v-btn>
                        </template>
                        <v-card>
                            <user-form v-bind:editedItem="editedItem"></user-form>
                            <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn color="primary" text @click="close">Cancel</v-btn>
                                <v-btn color="primary" text @click="save">Creae User</v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-dialog>
                </v-toolbar>
                </template>
            </v-data-table>
        </v-card>
        </v-container>
    </v-card>
</template>

<script>
import UserForm from "./UserForm"
import Auth from "../auth"

  export default {
    components: {
      UserForm
    },
    inject: ['userRepository'],
    data () {
      return {
        dialog: false,
        loading: true,
        isEditing: null,
        search: '',
        headers: [
        { text: 'First Name', value: 'firstName' },
        { text: 'Last Name', value: 'lastName' },
          { text: 'Username', value: 'username' },
          { text: 'Email', value: 'email' },
        ],
        users: [],
        firstUser: null,
        editedIndex: -1,
        editedItem: {
          displayName: '',
          email: '',
          username: '',
          password: ''
        },
        defaultItem: {
          displayName: '',
          email: '',
          username: '',
          password: ''
        }, 
      }
    },
    computed: {
        admin() {
            return Auth.isAdmin()
        },
    },

    watch: {
      dialog (val) {
        val || this.close()
      },
    },

    async created() {
      this.users = await this.userRepository.list();
      this.firstUser = await this.userRepository.find(1);
      this.loading = false;
    },

    methods: {
      close () {
        this.dialog = false
        setTimeout(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        }, 300)
      },

      save () {
        if (this.editedIndex > -1) {
          Object.assign(this.users[this.editedIndex], this.editedItem)
        } else {
          this.users.push(this.editedItem)
        }
        this.close()
      },
    },
  }
</script>