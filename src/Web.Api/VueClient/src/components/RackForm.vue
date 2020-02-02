<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
            <span class="headline">New Rack</span>
            </v-card-title>

            <v-card-text>
            <v-container>
                <v-row>
                <v-col cols="12" sm="6" md="6">
                    <v-text-field v-model="editedItem.rowStart" label="Row Letter Start"></v-text-field>
                </v-col>
                <v-col cols="12" sm="6" md="6">
                    <v-text-field v-model="editedItem.rowEnd" label="Row Letter End"></v-text-field>
                </v-col>
                </v-row>
                <v-row>
                <v-col cols="12" sm="6" md="6">
                    <v-text-field v-model="editedItem.rackStart" label="Rack Number Start"></v-text-field>
                </v-col>
                <v-col cols="12" sm="6" md="6">
                    <v-text-field v-model="editedItem.rackEnd" label="Rack Number End"></v-text-field>
                </v-col>
                </v-row>
            </v-container>
            </v-card-text>
        </v-card>
    </div>
</template>

<script>
export default {
    name: 'rack-form',
    inject: ['rackRepository'],
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
          rowStart: '',
          rowEnd: '',
          rackStart: '',
          rackEnd: ''
    },
}
</script>