<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
            <span class="headline">Edit</span>
            </v-card-title>
            <v-card-text>
            <v-container>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.vendor" label="Vendor" counter="50"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.modelNumber" label="Model Number" counter="50"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model.number="editedItem.height" label="Height" type="number"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model.number="editedItem.ethernetPorts" label="Ethernet Ports" type="number"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model.number="editedItem.powerPorts" label="Power Ports" type="number"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.cpu" label="CPU" counter="50"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model.number="editedItem.memory" label="Memory" type="number"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.storage" label="Storage"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.comment" label="Comment" multiLine textarea></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.displayColor" label="Display Color"></v-text-field>
                    </v-col>
                </v-row>
            </v-container>
            </v-card-text>
        </v-card>
    </div>
</template>

<script>
export default {
    name: 'model-edit',
    inject: ['modelRepository'],
    item: null,
    props: {
        editedItem : Object,
        models: Array,
    },
    data () {
        return {
            loading: false,
        };
    },
    editedIndex: -1,
}
</script>