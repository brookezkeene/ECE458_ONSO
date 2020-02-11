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
                            <v-combobox v-model="newItem.vendor"
                                        :items="models"
                                        item-text="vendor"
                                        item-value=""
                                        :return-object="false"
                                        label="Vendor"
                                        counter="50"
                                        clearable></v-combobox>
                        </v-col>
                        <v-col>
                        <v-text-field v-model="newItem.modelNumber" label="Model Number" counter="50"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="newItem.height" label="Height" type="number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="newItem.ethernetPorts" label="Ethernet Ports" type="number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="newItem.powerPorts" label="Power Ports" type="number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="newItem.cpu" label="CPU" counter="50"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="newItem.memory" label="Memory" type="number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="newItem.storage" label="Storage"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="newItem.comment" label="Comment" multiLine textarea></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-label>
                                Display Color
                            </v-label>
                            <v-color-picker v-model="newItem.displayColor">
                            </v-color-picker>
                        </v-col>
                    </v-row>

                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn text @click="close">Cancel</v-btn>
                        <v-btn color="primary" text @click="save">Save</v-btn>
                    </v-card-actions>'

                </v-container>
            </v-card-text>
        </v-card>
    </div>
</template>

<script>
    export default {
        name: 'model-edit',
        inject: ['modelRepository'],
        props: {
            id: String,
        },
        data() {
            return {
                models: [],
                loading: false,
                newItem: {
                    vendor: '',
                    modelNumber: '',
                    height: 0,
                    displayColor: '',
                    ehternetPorts: 0,
                    powerPorts: 0,
                    cpu: '',
                    memory: undefined,
                    storage: '',
                    comment: ''
                },
                isNew: 'false',

            };
        },
        editedIndex: -1,

        async created() {
            this.models = await this.modelRepository.list();

            this.newItem = typeof this.id === 'undefined'
                ? this.newItem
                : this.models.find(o => o.id === this.id);

        },

        computed: {
            formTitle() {
                return this.isNew ? 'New Item' : 'Edit Item'
            },
        },
        methods: {
            save() {
                if (!this.isNew) {
                    this.modelRepository.update(this.newItem);
                } else {
                    this.modelRepository.create(this.newItem);
                }
                this.close()
            },
            close() {
                setTimeout(() => {
                    this.$router.push({ name: 'model' })
                }, 300)
            },
        }
    }
</script>