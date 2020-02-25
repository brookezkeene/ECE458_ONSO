<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">{{formTitle}}</span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-expansion-panels multiple :hover=true :value="panel">
                        <v-expansion-panel v-for="(title, index) in titles"
                                            :key="title">
                            <v-expansion-panel-header>{{title}}</v-expansion-panel-header>
                            <v-expansion-panel-content>
                                <v-card flat v-if="index===0">
                                    <div align="center">
                                        <div>
                                            <v-row>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-autocomplete v-model="editedItem.modelId"
                                                                    :items="models"
                                                                    item-text="vendorModelNo"
                                                                    item-value="id"
                                                                    label="Model"
                                                                    @change="modelSelected()">
                                                    </v-autocomplete>
                                                </v-col>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-text-field v-model="editedItem.hostname"
                                                                  label="Host Name"
                                                                  counter="255">
                                                    </v-text-field>
                                                </v-col>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-text-field v-model="editedItem.assetNumber"
                                                                  label="Asset Number"
                                                                  counter="6">
                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </div>

                                        <div>
                                            <v-row>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-autocomplete v-model="editedItem.datacenter"
                                                                    label="Data Center"
                                                                    :items="datacenters"
                                                                    item-text="name"
                                                                    item-value="id">
                                                    </v-autocomplete>
                                                </v-col>
                                                <!-- Will need to update to show only racks from the selected datacenter -->
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-autocomplete v-if="!editedItem.datacenter.length==0 && updateRacks()"
                                                                    v-model="editedItem.rackId"
                                                                    label="Rack Number"
                                                                    :items="racks"
                                                                    item-text="address"
                                                                    item-value="id">
                                                    </v-autocomplete>
                                                </v-col>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-text-field v-if="!editedItem.datacenter.length==0 && updateRacks()"
                                                                    v-model.number="editedItem.rackPosition"
                                                                    label="Rack Position"
                                                                    type="number">
                                                    </v-text-field>
                                                </v-col>
                                            </v-row>
                                        </div>

                                        <div>
                                            <v-row>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-autocomplete v-model=editedItem.ownerId
                                                                    :items="users"
                                                                    item-text="username"
                                                                    item-value="id"
                                                                    label="Owner User Name"
                                                                    persistent-hint>
                                                    </v-autocomplete>
                                                </v-col>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-text-field v-model="editedItem.comment" label="Comment"></v-text-field>
                                                </v-col>
                                            </v-row>
                                        </div>
                                    </div>
                                </v-card>
                                <!-- MAC Addresses -->
                                <v-card class="overflow-y-auto"
                                        max-height="500px"
                                        flat 
                                        v-if="index===1">
                                    <div>
                                        <p v-if="editedItem.networkPorts.length > 0">Enter the MAC Address for each Network Port below.</p>
                                        <p v-else>No model selected. Please select a model first.</p>
                                    </div>
                                    <v-container fluid
                                                 fill
                                                 v-for="(port, index) in editedItem.networkPorts" :key="index">
                                        <v-layout align-center
                                                  justify-bottom>
                                            <v-spacer></v-spacer>
                                            <p>{{port.name}}</p>
                                            <v-spacer></v-spacer>
                                            <v-text-field :v-model="port.macAddress"
                                                          placeholde="MAC Address">
                                            </v-text-field>
                                            <v-spacer></v-spacer>
                                        </v-layout>
                                    </v-container>
                                </v-card>
                                <!-- Network Port Connections -->
                                <v-card class="overflow-y-auto"
                                        max-height="500px"
                                        flat 
                                        v-if="index===2">
                                    <div>
                                        <p v-if="editedItem.networkPorts.length > 0">Select another Network Port to connect to for each Network Port below.</p>
                                        <p v-else>No model selected. Please select a model first.</p>
                                    </div>
                                    <v-container fluid 
                                                 fill 
                                                  v-for="(port, index) in editedItem.networkPorts" :key="index">
                                        <v-layout align-center
                                                  justify-bottom>
                                            <v-spacer></v-spacer>
                                            <p>{{port.name}}</p>
                                            <v-spacer></v-spacer>
                                            <v-select :v-model="port.connectedPortId"
                                                      placeholder="Network Port">
                                            </v-select>
                                            <v-spacer></v-spacer>
                                        </v-layout>
                                    </v-container>
                                </v-card>
                                <!-- Power Ports and PDUs -->
                                <v-card class="overflow-y-auto"
                                        max-height="500px"
                                        flat 
                                        v-if="index===3">
                                    <div>
                                        <p v-if="editedItem.powerPorts.length > 0">Enter the PDU and PDU Number for each Power Port below.</p>
                                        <p v-else>No model selected. Please select a model first.</p>
                                    </div>
                                    <v-container fluid 
                                                 fill 
                                                 v-for="(port, index) in editedItem.powerPorts" :key="index">
                                        <v-layout align-center
                                                  justify-bottom>
                                                <v-spacer></v-spacer>
                                                <p>Power Port {{index+1}}</p>
                                                <v-spacer></v-spacer>
                                                <v-btn-toggle v-model="port.pduLocation"
                                                                mandatory>
                                                    <v-btn value="left">
                                                        Left
                                                    </v-btn>
                                                    <v-btn value="right">
                                                        Right
                                                    </v-btn>
                                                </v-btn-toggle>
                                                <v-spacer></v-spacer>
                                                <v-text-field v-model="port.pduNumber"
                                                              typeof="number"
                                                              placeholder="PDU Number">
                                                </v-text-field>
                                                <v-spacer></v-spacer>
                                        </v-layout>
                                    </v-container>
                                </v-card>
                            </v-expansion-panel-content>
                        </v-expansion-panel>
                    </v-expansion-panels>

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

<style>
    .main-div {
        position:relative; 
    }
    .bottom-div {
        vertical-align: bottom;
    }
</style>

<script>
    export default {
        name: 'asset-edit',
        inject: ['assetRepository', 'modelRepository', 'userRepository', 'rackRepository', 'datacenterRepository'],
        props: {
            id: String,
        },
        data() {
            return {
                models: [],
                assets: [],
                users: [],
                racks: [],
                datacenters: [],
                loading: false,
                titles: [
                    "Basic Asset Information",
                    "Mac Addresses",
                    "Network Port Connections",
                    "Power Connections",
                ],
                panel: [0],
                ownerId: '',
                editedItem: {
                    powerPorts: [],
                    networkPorts: [],
                    id: '',
                    rackId: '',
                    datacenter: '',
                    hostname: '',
                    comment: '',
                    rackPosition: 0,
                    ownerId: '',
                    modelId: '',
                    assetNumber: '000000'
                },
                datacenterID: '',
                rackNumber: 0,
                //networkPorts: [],
                //networkPortConnections: [],
                //powerPorts: [], // array of objects that contain fields port, location (left/right), and pduNumber
                toggle_exclusive: undefined,
                rules: {
                    powerPortRules: v => /^[a-zA-Z0-9]*$/.test(v) || 'Network port name cannot contain whitespace'
                },
                namesDialog: false,
            }
        }, 

        async created() {
            this.models = await this.modelRepository.list();
            this.assets = await this.assetRepository.list();
            this.users = await this.userRepository.list();
            this.racks = await this.rackRepository.list();
            this.datacenters = await this.datacenterRepository.list();

            const existingItem = await this.assets.find(o => o.id === this.id);
            if (typeof existingItem !== 'undefined') {
                this.editedItem = Object.assign({}, existingItem);
            }

            for (const model of this.models) {
                model.vendorModelNo = model.vendor + " " + model.modelNumber;
            }
        },

        computed: {
            formTitle() {
                return typeof this.id === 'undefined' ? 'New Item' : 'Edit Item'
            },
            powerPortNames() {
                var arr = [];
                var j;
                for (j = 0; j < 5; j++) {
                    arr[j] = j + 1;
                }
                return arr
            }

        },
        methods: {
            save() {
                if (typeof this.id !== 'undefined') {
                    this.assetRepository.update(this.editedItem).then(this.close());
                } else {
                    this.assetRepository.create(this.editedItem).then(this.close());
                }
            },
            close() {
                /* eslint-disable no-unused-vars, no-console */
                console.log(this.editedItem);
                this.$router.push({ name: 'assets' })
            },
           /* saveNames() {
                *//* eslint-disable no-unused-vars, no-console *//*
                console.log(this.networkPortNames);
                var i;
                for (i = 0; i < this.networkPortNames.length; i++) {
                    if (this.networkPortNames[i] === null) {
                        var portObjDefault = Object.assign({}, { name: (i + 1).toString(), number: i + 1 })
                        this.networkPorts[i] = portObjDefault;
                    } else {
                        var portObj = Object.assign({}, { name: this.networkPortNames[i], number: i + 1 })
                        this.networkPorts[i] = portObj;
                    }
                }
                console.log(this.networkPorts);
                this.namesDialog = false;
            },*/
            async updateRacks() {
                if (this.datacenterID != this.editedItem.datacenter) {
                    this.datacenterID = this.editedItem.datacenter;
                    this.racks = await this.rackRepository.list(this.datacenterID)
                    return true;
                }
                return false;
            },
            async modelSelected() {
                var selectedModel = await this.modelRepository.find(this.editedItem.modelId);
                /* eslint-disable no-unused-vars, no-console */
                console.log(selectedModel.networkPorts);
                var networkPortsArray = new Array();
                var j;
                for (j = 0; j < selectedModel.networkPorts.length; j++) {
                    const networkPortInfo = {
                        name: selectedModel.networkPorts[j].name,
                        number: selectedModel.networkPorts[j].number,
                        id: '',
                        macAddress: '',
                        connectedPortId: '',
                    }
                    var networkPortObj = Object.assign({}, networkPortInfo);
                    networkPortsArray.push(networkPortObj);
                }
                this.editedItem.networkPorts = networkPortsArray;

                var numPowerPorts = selectedModel.powerPorts;
                var powerPortsArray = new Array();
                var i;
                for (i = 0; i < numPowerPorts; i++) {
                    const powerPortInfo = {
                        number: i,
                        pduLocation: '',
                        pduNumber: 0,
                        pduPortId: ''
                    };
                    var powerPortObj = Object.assign({}, powerPortInfo);
                    powerPortsArray.push(powerPortObj);
                }
                this.editedItem.powerPorts = powerPortsArray;
                /* eslint-disable no-unused-vars, no-console */
                console.log(this.editedItem.networkPorts);
                console.log(this.editedItem.powerPorts);
            }
        }
    }
</script>
