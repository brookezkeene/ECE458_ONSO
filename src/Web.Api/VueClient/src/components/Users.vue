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
                      :loading="loading"
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
                      solo-inverted
                    ></v-autocomplete>
                    <v-spacer></v-spacer>

                    <v-btn v-if="admin" color="primary" dark class="mb-2" v-on="on" @click="openCreateUser">Add User</v-btn>

                </v-toolbar>
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
        this.initialize()
    },

    methods: {
        async initialize() {
            this.users = await this.userRepository.list();
            this.loading = false;
        },
        openCreateUser() {
            this.$router.push({ name: 'users-create' });
        }
    },
  }
</script>