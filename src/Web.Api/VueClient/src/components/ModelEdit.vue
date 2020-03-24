<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">{{formTitle}}</span>
            </v-card-title>

            <v-container>
                <v-form v-model="valid">
                    <v-row>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="newItem.vendor"
                                        item-text="vendor"
                                        item-value=""
                                        :return-object="false"
                                        label="Vendor"
                                        placeholder="Please enter a vendor (i.e. Dell)"
                                        :rules="[rules.vendorRules]"
                                        counter="50"
                                        required></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="newItem.modelNumber"
                                          label="Model Number"
                                          placeholder="Please enter a model number (i.e. R710)"
                                          :rules="[rules.modelRules]"
                                          counter="50"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="newItem.height"
                                          label="Height (in Rack U)"
                                          placeholder="Please enter a height"
                                          type="number"
                                          :rules="[rules.heightRules]"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="newItem.ethernetPorts" label="Network Ports" type="number" @change="networkPortNum"
                                          :rules="[rules.ethernetPortRules]"></v-text-field>
                            <a href="#" @click="openNamesDialog">Add Network Port Names</a>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="newItem.powerPorts" label="Power Ports" type="number"
                                          :rules="[rules.powerPortRules]"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="newItem.cpu" label="CPU" placeholder="i.e. Intel Xeon E5520 2.2GHz" counter="50"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model.number="newItem.memory" label="Memory (in GB)" type="number"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="4">
                            <v-text-field v-model="newItem.storage" label="Storage" placeholder="2x500GB SSD RAID1"></v-text-field>
                        </v-col>
                    </v-row>

                    <v-row>
                        <v-col cols="6">
                            <v-textarea v-model="newItem.comment" label="Comment" multi-line textarea></v-textarea>
                        </v-col>
                    </v-row>

                    <v-row>
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
                        <v-btn @click="close">Cancel</v-btn>
                        <v-btn color="primary" :disabled="!valid" @click="save">Save</v-btn>
                    </v-card-actions>'
                </v-form>
            </v-container>

            <template>
                <!-- dialog to set network port names -->

                <div class="text-center">
                    <v-dialog v-model="namesDialog" width="400">
                        <v-card class="overflow-y-auto" max-height="500px">
                            <v-card-title>
                                Edit Network Port Names
                            </v-card-title>
                            <v-card-text>
                                <v-container fluid>
                                    <div v-for="(n, index) in newItem.ethernetPorts" :key="index">
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
                                <v-btn color="primary" text @click="closeNamesDialog">Close</v-btn>
                                <v-btn color="primary" text @click="saveNames">Save</v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-dialog>
                </div>
            </template>
            <v-snackbar v-model="updateSnackbar.show"
                        :bottom=true
                        class="black--text"
                        :color="updateSnackbar.color"
                        :timeout=5000>
                {{updateSnackbar.message}}
                <v-btn dark
                       class="black--text"
                       text
                       @click="updateSnackbar.show = false">
                    Close
                </v-btn>
            </v-snackbar>
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
                updateSnackbar: {
                    show: false,
                    message: '',
                    color: ''
                },
                loading: false,
                newItem: {
                    vendor: '',
                    modelNumber: '',
                    height: 0,
                    displayColor: '',
                    ethernetPorts: 0,
                    powerPorts: 0,
                    cpu: '',
                    memory: 0,
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
                    vendorRules: v => /^(?=\s*\S).*$/.test(v) || 'Vendor is required',
                    modelRules: v => /^(?!\s*$).+/.test(v) || 'Model Number is required',
                    heightRules: v => /^(?=\s*\S).*$/.test(v) && v > 0 || 'Height is required',
                    networkPortRules: v => /^[a-zA-Z0-9]*$/.test(v) || 'Network port name cannot contain whitespace',
                    ethernetPortRules: v => /^(?=\s*\S).*$/.test(v) || 'Number of ethernet ports must be a positive valid number.',
                    powerPortRules: v => /^(?=\s*\S).*$/.test(v) || 'Number of power ports must be a positive valid number.'


                },
                valid: true
            };
        },

        async created() {

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
            async save() {
                if (this.validationInputs(this.newItem) > 0) {
                    return;
                }
                if (typeof this.id !== 'undefined') {
                    this.newItem.displayColor = this.newItem.displayColor.substring(0, 7);

                    var resultUpdate = await this.modelRepository.update(this.newItem);
                    if (this.validationCreateAndUpdate(resultUpdate) != 0) {
                        return;
                    }

                } else {
                    this.newItem.displayColor = this.newItem.displayColor.substring(0, 7);

                    var resultCreate = await this.modelRepository.create(this.newItem);
                    if (this.validationCreateAndUpdate(resultCreate) != 0) {
                        return;
                    }
                }
                this.close()
            },
            close() {
                setTimeout(() => {
                    this.$router.push({ name: 'models' })
                }, 300)
            },
            networkPortNum() {
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
            },
            openNamesDialog() {
                this.namesDialog = true;
            },
            /*here is where networkPortNames modifies the newItem.networkPorts
              to update the values of the item*/
            saveNames() {

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

                this.namesDialog = false;
            },
            closeNamesDialog() {
                this.namesDialog = false;
            },
            validationInputs(item) {
                var count = 0;
                this.updateSnackbar.message = '';
                if (item.vendor == null || item.vendor.length == 0) {
                    this.updateSnackbar.show = true;
                    this.updateSnackbar.color = 'red lighten-4';
                    this.updateSnackbar.message = this.updateSnackbar.message + 'Vendor name field is required. ';
                    count++;
                }
                if (item.modelNumber == null || item.modelNumber.length == 0) {
                    this.updateSnackbar.show = true;
                    this.updateSnackbar.color = 'red lighten-4';
                    this.updateSnackbar.message = this.updateSnackbar.message + 'Model Number field is required. ';
                    count++;
                }
                if (item.height <= 0 || item.height > 42 || !(/^[0-9]*$/.test(item.height))) {
                    this.updateSnackbar.show = true;
                    this.updateSnackbar.color = 'red lighten-4';
                    this.updateSnackbar.message = this.updateSnackbar.message + 'The height of the model must be a valid number greater than 0 and less than 42. ';
                    count++
                }
                if (item.memory < 0 || !(/^[0-9]*$/.test(item.memory))) {
                    this.updateSnackbar.show = true;
                    this.updateSnackbar.color = 'red lighten-4';
                    this.updateSnackbar.message = this.updateSnackbar.message + 'Memory must be a valid number greater than -1. ';
                    count++
                }
                return count;
            },
            validationCreateAndUpdate(result) {
                if (result != null && result.length != 0) {
                    this.updateSnackbar.show = true;
                    this.updateSnackbar.color = 'red lighten-4';
                    this.updateSnackbar.message = 'Failed to create model. Model vendor and model number must be unique';
                    return 1;
                }
                return 0;
            }
        }
    }
</script>
