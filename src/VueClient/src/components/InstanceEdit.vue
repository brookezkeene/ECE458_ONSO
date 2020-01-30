<template>
    <div v-if="!loading">
        <v-card color="blue" dark>
            <v-card-title>
            <span class="headline">Edit</span>
            </v-card-title>
            <v-card-text>
            <v-container>
                <v-row no-gutters>
                <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.hostname" label="Host Name"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.rack" label="Rack Number"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.rackPosition" label="Rack Position"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.owner.id" label="Owner ID"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.owner.username" label="Owner User Name"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="editedItem.owner.displayName" label="Owner Display Name"></v-text-field>
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
    inject: ['instanceRepository'],
    item: null,
    props: {
        editedItem : Object
    },
    data () {
        return {
            loading: false,
        };
    },
    editedIndex: -1,
    editedItem: {
        //model:'',
        hostname: '',
        rack:'',
        rackPosition:'',
        owner:'',
        comment: ''
    },
}
</script>