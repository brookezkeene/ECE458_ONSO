<template>
    <div>
        <v-card flat>
            <v-card-title>Models</v-card-title>
            <v-container>
            <v-card>
                <v-data-table :headers="filteredHeaders"
                              :items="models"
                              :search="search"
                              multi-sort
                              @click:row="showDetails">
                    <template v-slot:top v-slot:item.action="{ item }">

                        <v-toolbar flat color="white">
                            <v-autocomplete prepend-inner-icon="mdi-magnify"
                                            :search-input.sync="search"
                                            cache-items
                                            flat
                                            hide-no-data
                                            hide-details
                                            label="Search"
                                            single-line
                                            solo-inverted></v-autocomplete>
                            <v-spacer></v-spacer>
                            <v-dialog v-model="dialog" max-width="500px">
                                <template v-slot:activator="{ on }">
                                    <v-btn v-if="admin" color="primary" class="mb-2" v-on="on">Add Model</v-btn>
                                </template>
                                <v-card>
                                    <model-edit v-bind:editedItem="editedItem" v-bind:models="models"></model-edit>
                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="primary" text @click="close">Cancel</v-btn>
                                        <v-btn color="primary" text @click="save">Save</v-btn>
                                    </v-card-actions>
                                </v-card>
                            </v-dialog>
                        </v-toolbar>

                        <v-row class="pl-4">
                            <v-col cols="10">
                                <v-row>
                                    <!-- Custom filters; sorts between height ranges -->
                                    <v-col cols="3">
                                        <v-row>
                                            <v-col cols="6">
                                                <v-text-field v-model="startHeightValue"
                                                              placeholder="from"
                                                              type="number"
                                                              label="Height">
                                                </v-text-field>
                                            </v-col>
                                            <v-col cols="6">
                                                <v-text-field v-model="endHeightValue"
                                                              type="number"
                                                              placeholder="to">
                                                </v-text-field>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                    <v-col cols="3">
                                        <v-row>
                                            <v-col cols="6">
                                                <v-text-field v-model="startMemoryValue"
                                                              placeholder="from"
                                                              type="number"
                                                              label="Memory">
                                                </v-text-field>
                                            </v-col>
                                            <v-col cols="6">
                                                <v-text-field v-model="endMemoryValue"
                                                              type="number"
                                                              placeholder="to">
                                                </v-text-field>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                    <v-col cols="3">
                                        <v-row>
                                            <v-col cols="6">
                                                <v-text-field v-model="startEthernetValue"
                                                              placeholder="from"
                                                              type="number"
                                                              label="Ethernet Ports">
                                                </v-text-field>
                                            </v-col>
                                            <v-col cols="6">
                                                <v-text-field v-model="endEthernetValue"
                                                              type="number"
                                                              placeholder="to">
                                                </v-text-field>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                    <v-col cols="3">
                                        <v-row>
                                            <v-col cols="6">
                                                <v-text-field v-model="startPowerValue"
                                                              placeholder="from"
                                                              type="number"
                                                              label="Power Ports">
                                                </v-text-field>
                                            </v-col>
                                            <v-col cols="6">
                                                <v-text-field v-model="endPowerValue"
                                                              type="number"
                                                              placeholder="to">
                                                </v-text-field>
                                            </v-col>
                                        </v-row>
                                    </v-col>
                                </v-row>
                            </v-col>
                        </v-row>

                    </template>

                    <template v-slot:item.coloricon="{ item }">
                        <v-icon class="mr-2"
                                :color=item.displayColor>
                            mdi-circle
                        </v-icon>
                    </template>

                    <template v-if="admin" v-slot:item.action="{ item }">
                        <v-row>
                            <v-icon medium
                                    class="mr-2"
                                    @click="editItem(item)">mdi-pencil</v-icon>
                            <v-icon medium
                                    class="mr-2"
                                    @click="deleteItem(item)">mdi-delete</v-icon>
                        </v-row>
                    </template>
                    <template v-slot:no-data>
                        <v-btn color="primary" @click="initialize">Refresh</v-btn>
                    </template>
                    >
                </v-data-table>
            </v-card>
            </v-container>

        </v-card>
    </div>
</template>


<script>
    import ModelEdit from "./ModelEdit"
    import Auth from "../auth"

    export default {
        components: {
            ModelEdit,
        },
        inject: ['modelRepository'],
        data() {
            return {
                // Filter values.
                startHeightValue: '',
                endHeightValue: '',
                startMemoryValue: '',
                endMemoryValue: '',
                startEthernetValue: '',
                endEthernetValue: '',
                startPowerValue: '',
                endPowerValue: '',

                dialog: false,
                detailsDialog: false,
                loading: true,
                search: '',
                headers: [
                    {
                        text: 'Vendor',
                        align: 'left',
                        value: 'vendor'
                    },
                    { text: 'Model Number', value: 'modelNumber', },
                    { text: 'Height', value: 'height', filter: this.heightFilter },
                    { text: 'Display Color', value: 'coloricon', sortable: false },
                    { text: 'Ethernet Ports', value: 'ethernetPorts', filter: this.ethernetFilter },
                    { text: 'Power Ports', value: 'powerPorts', filter: this.powerFilter },
                    { text: 'CPU', value: 'cpu'},
                    { text: 'Memory', value: 'memory', filter: this.memoryFilter },
                    { text: 'Storage', value: 'storage' },
                    { text: 'Comment', value: 'comment' },
                    { text: 'Actions', value: 'action', sortable: false },

                ],
                models: [],
                editedIndex: -1,
                editedItem: {
                    vendor: '',
                    modelNumber: '',
                    height: 0,
                    displayColor: '',
                    ethernetPorts: 0,
                    powerPorts: 0,
                    cpu: '',
                    memory: 0,
                    storage: '',
                    comment: ''
                },
                defaultItem: {
                    vendor: '',
                    modelNumber: '',
                    height: 0,
                    displayColor: '',
                    ethernetPorts: 0,
                    powerPorts: 0,
                    cpu: '',
                    memory: 0,
                    storage: '',
                    comment: ''
                },
                detailItem: {
                    comment: ''
                },
                deleting: false,
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
            },
            admin() {
                return Auth.isAdmin()
            },
            filteredHeaders() {
                return (this.admin) ? this.headers : this.headers.filter(h => h.text !== "Actions")
            },
        },
        watch: {
            dialog(val) {
                val || this.close()
            },
            detailsDialog(val) {
                val || this.closeDetail()
            },
        },
        async created() {
            this.initialize()
        },

        methods: {
            async initialize() {
                this.models = await this.modelRepository.list();
                this.loading = false;
            },
            editItem(item) {
                this.editedIndex = this.models.indexOf(item)
                this.editedItem = Object.assign({}, item)
                this.dialog = true
            },
            deleteItem(item) {
                this.deleting = true;
                confirm('Are you sure you want to delete this item?') && this.modelRepository.delete(item)
                      .then(async () => {
                            await this.initialize();
                        })
            },
            close() {
                this.dialog = false
                setTimeout(() => {
                    this.editedItem = Object.assign({}, this.defaultItem)
                    this.editedIndex = -1
                }, 300)

            },
            save() {
                if (this.editedIndex > -1) {
                    this.modelRepository.update(this.editedItem)
                        .then(async () => {
                            await this.initialize();
                        })
                } else {
                    this.modelRepository.create(this.editedItem)
                        .then(async () => {
                            await this.initialize();
                        })
                }
                this.close()
            },
            showDetails(item) {
                if (this.editedIndex === -1 && !this.deleting) {
                    this.detailItem = Object.assign({}, item);
                    this.$router.push({ name: 'model-details', params: { detailItem: this.detailItem, id: this.detailItem.id } })
                }
                this.deleting = false;
                //this.detailsDialog = true;
            },
            //closeDetail() {
            //    this.detailsDialog = false;
            //},
          /**
           * Filter code below; TODO: refactor this 
           * end/start values are the filter inputs 
           * @param value is the value of the table entry
           */
          heightFilter(value) {
            // If this filter has no value we just skip the entire filter.
            if (!this.startHeightValue && !this.endHeightValue) {
              return true;
            // If only one filter has a value, leans entirely on that one filter
            } else if (!this.endHeightValue) {
              return (value) >= parseInt(this.startHeightValue);
            } else if (!this.startHeightValue) {
              return (value) <= parseInt(this.endHeightValue);
            }  
 
            // Check if the current loop value (The Height value)
            // is between the Height values inputted
            return (value) >= parseInt(this.startHeightValue)
                    && (value) <= parseInt(this.endHeightValue);
          },
          memoryFilter(value) {
            if (!this.startMemoryValue && !this.endMemoryValue) {
              return true;
            } else if (!this.endMemoryValue) {
              return (value) >= parseInt(this.startMemoryValue);
            } else if (!this.startMemoryValue) {
              return (value) <= parseInt(this.endMemoryValue);
            }  

 
            return (value) >= parseInt(this.startMemoryValue)
                    && (value) <= parseInt(this.endMemoryValue);
            },
          ethernetFilter(value) {
            if (!this.startEthernetValue && !this.endEthernetValue) {
              return true;
            } else if (!this.endEthernetValue) {
              return (value) >= parseInt(this.startEthernetValue);
            } else if (!this.startEthernetValue) {
              return (value) <= parseInt(this.endEthernetValue);
            }  
 
            return (value) >= parseInt(this.startEthernetValue)
                    && (value) <= parseInt(this.endEthernetValue);
          },
          powerFilter(value) {
            if (!this.startPowerValue && !this.endPowerValue) {
              return true;
            } else if (!this.endPowerValue) {
              return (value) >= parseInt(this.startPowerValue);
            } else if (!this.startPowerValue) {
              return (value) <= parseInt(this.endPowerValue);
            }  
 
            return (value) >= parseInt(this.startPowerValue)
                    && (value) <= parseInt(this.endPowerValue);
            },


        },
    }
</script>