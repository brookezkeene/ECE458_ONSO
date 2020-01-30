<template>
    <div v-if="!loading">
        <v-card color="blue" dark>
            <v-card-title>
            <span class="headline">Details</span>
            </v-card-title>
            <v-card-text>
            <v-container>
                <v-row no-gutters>
                <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="detailItem.hostname" label="Host Name"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="detailItem.rack" label="Rack Number"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="detailItem.rackPosition" label="Rack Position"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="detailItem.owner.id" label="Owner ID"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="detailItem.owner.username" label="Owner User Name"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="detailItem.owner.displayName" label="Owner Display Name"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="detailItem.owner.email" label="Owner email"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field v-model="detailItem.comment" label="Comment"></v-text-field>
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
        detailItem : Object
    },
    data () {
        return {
            loading: false,
        };
    },
    detailItem : {
          hostname:'',
          rack:'',
          rackPosition:'',
          owner:'',
          comment: ''
    },
}
</script>