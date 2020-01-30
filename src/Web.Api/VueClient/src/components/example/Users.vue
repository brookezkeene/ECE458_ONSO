<template>
  <v-card>
    <v-card-title>
      Users
      <v-spacer></v-spacer>
      <v-text-field
        v-model="search"
        label="Search"
        single-line
        hide-details
      ></v-text-field>
    </v-card-title>
    <v-data-table
      :headers="headers"
      :items="users"
      :search="search"
      multi-sort
    ></v-data-table>
    <v-card v-if="!loading">
      <v-card-title>
        find(...) test
      </v-card-title>
      <v-card-text>
        <p>Display name: {{ firstUser.displayName }}</p>
        <p>Username: {{ firstUser.username }}</p>
        <p>Email: {{ firstUser.email }}</p>
      </v-card-text>
    </v-card>
  </v-card>
</template>

<script>
  export default {
    inject: ['userRepository'],
    data () {
      return {
        loading: true,
        search: '',
        headers: [
          { text: 'Display name', value: 'displayName' },
          { text: 'Username', value: 'username' },
          { text: 'Email', value: 'email' }
        ],
        users: [],
        firstUser: null
      }
    },
    async created() {
      this.users = await this.userRepository.list();
      this.firstUser = await this.userRepository.find(1);
      this.loading = false;
    }

  }
</script>