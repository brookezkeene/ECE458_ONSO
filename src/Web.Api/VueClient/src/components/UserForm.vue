<template>
    <div v-if="!loading">
        <v-card>
            <v-card-title>
            <span class="headline">New User</span>
            </v-card-title>

            <v-card-text>
            <v-container>
                <v-row>
                <v-col cols="12" sm="6" md="6">
                    <v-text-field v-model="editedItem.displayName" label="Display Name"></v-text-field>
                </v-col>
                </v-row>
                <v-row>
                <v-col cols="12" sm="6" md="6">
                    <v-text-field v-model="editedItem.username" label="Username"></v-text-field>
                </v-col>
                </v-row>
                <v-row>
                <v-col cols="12" sm="6" md="6">
                    <v-text-field v-model="editedItem.email" label="Email"></v-text-field>
                </v-col>
                </v-row>
            </v-container>
            </v-card-text>
        </v-card>
    </div>
</template>

<script>
export default {
    name: 'user-form',
    inject: ['userRepository'],
    item: null,
    props: {
        editedItem: Object
    },
    data () {
        return {
            loading: false
        };
    },
    editedItem: {
          displayName: '',
          email: '',
          username: '',
          password: ''
    },
}
</script>