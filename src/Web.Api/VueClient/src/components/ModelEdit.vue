<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
            <span class="headline">Edit</span>
            </v-card-title>
            <v-card-text>
            <v-container>
                <v-row no-gutters>
                    <v-col cols="12" sm="6" md="4">
                        <v-combobox v-model="editedItem.vendor"
                                    :items="models"
                                    item-text="vendor"
                                    label="Vendor"
                                    counter="50"></v-combobox>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.modelNumber" label="Model Number" counter="50"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.height" label="Height"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.ethernetPorts" label="Ethernet Ports"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.powerPorts" label="Power Ports"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.cpu" label="CPU" counter="50"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.memory" label="Memory"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.storage" label="Storage"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-text-field v-model="editedItem.comment" label="Comment"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>
                            Display Color
                        </v-label>
                        <v-color-picker v-model="editedItem.displayColor">
                        </v-color-picker>
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