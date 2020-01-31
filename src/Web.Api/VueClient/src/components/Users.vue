<template>
    <v-card>
        <v-data-table
            :headers="headers"
            :items="users"
            :search="search"
            multi-sort
        >
            <template v-slot:top>
            <v-toolbar flat color="white">
                <v-toolbar-title>Users</v-toolbar-title>
                <v-divider
                  class="mx-4"
                  inset
                  vertical
                ></v-divider>
                <v-spacer></v-spacer>

                <v-autocomplete
                  v-model="select"
                  :loading="loading"
                  :items="users"
                  :search-input.sync="search"
                  append-icon="mdi-magnify"
                  cache-items
                  class="mx-4"
                  flat
                  hide-no-data
                  hide-details
                  item-text="displayName"
                  label="Search"
                  single-line
                  solo-inverted
                ></v-autocomplete>

                <v-dialog v-model="dialog" max-width="500px">
                <template v-slot:activator="{ on }">
                    <v-btn color="primary" dark class="mb-2" v-on="on">Add User</v-btn>
                </template>
                    <user-form v-bind:editedItem="editedItem"></user-form>
                    <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="primary" text @click="close">Cancel</v-btn>
                    <v-btn color="primary" text @click="save">Creae User</v-btn>
                    </v-card-actions>
                </v-dialog>
            </v-toolbar>
            </template>
        </v-data-table>
    </v-card>
</template>

<script>
import UserForm from "./UserForm"

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
          { text: 'Display Name', value: 'displayName' },
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
        if (this.editedIndex > -1 && confirm('This will send an email to the new user to set up their account')) {
          Object.assign(this.users[this.editedIndex], this.editedItem)
        } else if(confirm('This will send an email to the new user to set up their account')) {
          this.users.push(this.editedItem)
        }
        this.close()
      },
    },
  }
</script>