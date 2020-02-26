<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">{{formTitle}}</span>
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
                            <v-text-field v-model.number="newItem.ethernetPorts" label="Network Ports" type="number"></v-text-field> <!--networkPorts-->
                            <a href="#" @click="openNamesDialog">Add Network Port Names</a>
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
                            <v-color-picker v-model="color">
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

            <template> <!-- dialog to set network port names -->
                <div class="text-center">
                    <v-dialog v-model="namesDialog" width="400">
                        <v-card class="overflow-y-auto" max-height="500px">
                            <v-card-title>
                                Edit Network Port Names
                            </v-card-title>
                            <v-card-text>
                                <v-container fluid>
                                    <div v-for="(n, index) in this.networkPortNames" :key="n">
                                        <v-text-field v-model="networkPortNames[index]" 
                                                      label="Network Port" 
                                                      placeholder="port name"
                                                      :rules="[rules.networkPortRules]"
                                                      :value="n"></v-text-field>
                                    </div>
                                </v-container>
                            </v-card-text>
                            <v-card-actions>
                                <v-spacer></v-spacer>
                                <v-btn color="primary" text @click="saveNames">Save</v-btn>
                                <v-btn color="primary" text @click="closeNamesDialog">Close</v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-dialog>
                </div>
            </template>
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
        data: () => {
            return {
                models: [],
                color: '',
                loading: false,
                newItem: {
                    vendor: '',
                    modelNumber: '',
                    height: 0,
                    displayColor: '',
                    ethernetPorts: 0, // networkPorts: 0,
                    powerPorts: 0,
                    cpu: '',
                    memory: undefined,
                    storage: '',
                    comment: '',
                    networkPorts: []
                },
                namesDialog: false,
                editedIndex: -1,
                //making this a separate variable: stores the CURRENT names of network ports
                //for UPDATE: if the user changes the size of the ethernetports/closes the 
                //names dialog, the newItem will still have original value from database
                networkPortNames: [],
                rules: {
                    networkPortRules: v => /^[a-zA-Z0-9]*$/.test(v) || 'Network port name cannot contain whitespace'
                },
            };
        },

        async created() {
            this.models = await this.modelRepository.list();

            this.newItem = typeof this.id === 'undefined'
                ? this.newItem
                : await this.modelRepository.find(this.id);

        },
        computed: {
            formTitle() {
                return typeof this.id === 'undefined' ? 'New Item' : 'Edit Item'
            },
        },
        methods: {
            save() {
                if (typeof this.id !== 'undefined') {
                    this.newItem.displayColor = this.color.substring(0, 7);
                    this.modelRepository.update(this.newItem);
                } else {
                    this.newItem.displayColor = this.color.substring(0, 7);
                    this.modelRepository.create(this.newItem);
                }
                this.close()
            },
            close() {
                setTimeout(() => {
                    this.$router.push({ name: 'models' })
                }, 300)
            },
            openNamesDialog() {
                //setting default values of the networkPortNames
                //either 1) creating a model, populating ports with default names
                // 2) updating the networkPortNames with the newItem.ethernetPorts from memory
                if (typeof this.id === 'undefined') {
                    var j; 
                    for (j = 0; j < this.newItem.ethernetPorts; j++) {
                        if (this.networkPortNames[j] == null) {
                            this.networkPortNames[j] = (j + 1).toString();
                        }
                    }
                    if (this.newItem.ethernetPorts < this.networkPortNames.length) {
                        this.networkPortNames = this.networkPortNames.slice(0, this.newItem.ethernetPorts);
                    }
                } else {
                    var i;
                    for (i = 0; i < this.newItem.ethernetPorts; i++) {
                        if (this.newItem.networkPorts.length > (i)) {
                            this.networkPortNames[i] = this.newItem.networkPorts[i].name;
                        } else {
                            this.networkPortNames[i] = (i + 1).toString();
                        }
                    }
                    if (this.newItem.ethernetPorts < this.networkPortNames.length) {
                        this.networkPortNames = this.networkPortNames.slice(0, this.newItem.ethernetPorts);
                    }
                }
                this.namesDialog = true;
            },
            /*here is where networkPortNames modifies the newItem.networkPorts 
              to update the values of the item*/
            saveNames() {
                 /* eslint-disable no-unused-vars, no-console */
                console.log(this.networkPortNames);

                var i;
                for (i = 0; i < this.networkPortNames.length; i++) {
                    if (typeof this.id === 'undefined' && this.newItem.networkPorts.length == 0) {
                        var portObj = Object.assign({}, { name: this.networkPortNames[i], number: i + 1 })
                        this.newItem.networkPorts.push(portObj);
                    } else if (this.newItem.networkPorts.length <= i) {
                        var addObj = Object.assign({}, { name: this.networkPortNames[i], number: i + 1 })
                        this.newItem.networkPorts.push(addObj);
                    } else {
                        this.newItem.networkPorts[i].name = this.networkPortNames[i];
                    }
                }
                //for updating: if user decreased the ethernetport size
                if (this.newItem.networkPorts.length > this.newItem.ethernetPorts) {
                    this.newItem.networkPorts = this.newItem.networkPorts.slice(0, this.newItem.ethernetPorts);
                }

                                console.log(this.newItem.networkPorts);

                
                this.namesDialog = false;
            },
            closeNamesDialog() {
                this.namesDialog = false;
            },
        }
    }
</script>