<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">{{formTitle}}</span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-form v-model="valid">
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
                                                                        placeholder="Please select an existing model"
                                                                        :rules="[rules.modelRules]"
                                                                        @change="modelSelected()">
                                                        </v-autocomplete>
                                                    </v-col>
                                                    <v-col cols="12" sm="6" md="4">
                                                        <v-text-field v-model="editedItem.hostname"
                                                                      label="Host Name"
                                                                      placeholder="Please enter a RFC 1034 compliant hostname"
                                                                      :rules="[rules.hostnameRules]"
                                                                      counter="255">
                                                        </v-text-field>
                                                    </v-col>
                                                    <v-col cols="12" sm="6" md="4">
                                                        <v-text-field v-model="editedItem.assetNumber"
                                                                      label="Asset Number"
                                                                      placeholder="Please enter a 6 digit serial number"
                                                                      :rules="[rules.assetRules]"
                                                                      counter="6">
                                                        </v-text-field>
                                                    </v-col>
                                                </v-row>
                                            </div>

                                            <div>
                                                <v-row>
                                                    <v-col cols="12" sm="6" md="4">
                                                        <v-autocomplete v-model="editedItem.datacenterId"
                                                                        label="Data Center"
                                                                        placeholder="Please select an existing datacenter"
                                                                        :rules="[rules.datacenterRules]"
                                                                        :items="datacenters"
                                                                        item-text="name"
                                                                        item-value="id">
                                                        </v-autocomplete>
                                                    </v-col>
                                                    <!-- Will need to update to show only racks from the selected datacenter -->
                                                    <v-col cols="12" sm="6" md="4">
                                                        <v-autocomplete v-if="!editedItem.datacenterId.length==0 && updateRacks()"
                                                                        v-model="editedItem.rackId"
                                                                        label="Rack Number"
                                                                        placeholder="Please select a rack"
                                                                        :rules="[rules.rackRules]"
                                                                        :items="racks"
                                                                        item-text="address"
                                                                        item-value="id"
                                                                        @change="rackSelected">
                                                        </v-autocomplete>
                                                    </v-col>
                                                    <v-col cols="12" sm="6" md="4">
                                                        <v-text-field v-if="!editedItem.datacenterId.length==0 && updateRacks()"
                                                                      v-model.number="editedItem.rackPosition"
                                                                      placeholder="Please enter a rack U for the asset"
                                                                      :rules="[rules.rackuRules]"
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
                                                </v-row>
                                                <v-row>
                                                    <v-col cols="6">
                                                        <v-textarea v-model="editedItem.comment" label="Comment" multi-line textarea></v-textarea>
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
                                                     v-for="(port, index) in networkPorts" :key="index">
                                            <v-layout align-center
                                                      justify-bottom>
                                                <v-spacer></v-spacer>
                                                <p>{{port.name}}</p>
                                                <v-spacer></v-spacer>
                                                <v-text-field v-model="editedItem.networkPorts[index].macAddress"
                                                              placeholder="Please enter a 6-byte hexadecimal string"
                                                              :rules="[rules.macAddressRules]">
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
                                                     v-for="(port, index) in networkPorts" :key="index">
                                            <v-layout align-center
                                                      justify-bottom>
                                                <v-spacer></v-spacer>
                                                <p>{{port.name}}</p>
                                                <v-spacer></v-spacer>
                                                <v-autocomplete v-model="editedItem.networkPorts[index].connectedPortId"
                                                                :items="networks"
                                                                item-text="nameRackAssetNum"
                                                                item-value="id"
                                                                label="Connected Network Port"
                                                                persistent-hint
                                                                class="overflow-x-auto">
                                                </v-autocomplete>
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
                                            <p v-if="(selectedRack && selectedModelBool) || (id!=undefined)">Enter the PDU and PDU Number for each Power Port below.</p>
                                            <p v-else>No model or rack selected. Please select a model and a rack first.</p>
                                        </div>
                                        <div v-if="(selectedRack && selectedModelBool) || (id!=undefined)">

                                            <v-container fluid
                                                         fill
                                                         v-for="(port, index) in powerPorts" :key="index">
                                                <v-layout align-center
                                                          justify-bottom>
                                                    <v-spacer></v-spacer>
                                                    <v-spacer></v-spacer>

                                                    <p>Power Port {{index+1}}</p>
                                                    <v-spacer></v-spacer>
                                                    <v-btn-toggle v-model="port.pduLocation"
                                                                  mandatory>
                                                        <v-btn @click="setLocation('left')" value="true">
                                                            Left
                                                        </v-btn>
                                                        <v-btn @click="setLocation('right')" value="false">
                                                            Right
                                                        </v-btn>
                                                    </v-btn-toggle>
                                                    <v-spacer></v-spacer>
                                                    <v-autocomplete v-if="port.pduLocation" 
                                                                v-model="editedItem.powerPorts[index].pduPortId"
                                                                :items="availablePortsInRack.right"
                                                                item-text ="number"
                                                                item-value ="id"
                                                                :return-object="false"
                                                                typeof="number"
                                                                placeholder="PDU Number">
                                                    </v-autocomplete>
                                                    <v-autocomplete v-if="!port.pduLocation" 
                                                                v-model="editedItem.powerPorts[index].pduPortId"
                                                                :items="availablePortsInRack.left"
                                                                item-text="number"
                                                                item-value="id"
                                                                :return-object="false"
                                                                typeof="number"
                                                                placeholder="PDU Number">
                                                    </v-autocomplete>
                                                    <v-spacer></v-spacer>
                                                </v-layout>
                                            </v-container>
                                        </div>
                                    </v-card>
                            </v-expansion-panel-content>
                        </v-expansion-panel>
                    </v-expansion-panels>

                        <v-card-actions>
                            <v-spacer></v-spacer>
                            <v-btn @click="close">Cancel</v-btn>
                            <v-btn color="primary" :disabled="!valid" @click="save">Save</v-btn>
                        </v-card-actions>'
                        </v-form>
                </v-container>
            </v-card-text>
        </v-card>
    </div>
</template>

<style>
    .main-div {
        position: relative;
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
                networks: [],
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
                    datacenterId: '',
                    hostname: '',
                    comment: '',
                    rackPosition: 0,
                    ownerId: '',
                    modelId: '',
                    assetNumber: ''
                },
                selectedModel: [],
                datacenterID: '',
                rackNumber: 0,
                networkPorts: [], // array of objects that contains name and number
                powerPorts: [], // array of objects that contain fields port, location (left/right), and pduNumber
                //networkPortConnections: [],
                toggle_exclusive: undefined,
                namesDialog: false,
                selectedRack: false,
                selectedModelBool: false,
                sidePortIsOn: 'left',
                availablePortsInRack: [],
                rules: {
                    modelRules: v => /^(?!\s*$).+/.test(v) || 'Model is required',
                    hostnameRules: v => /^(?![0-9]+$)(?!.*-$)(?!-)[a-zA-Z0-9-]{1,63}$/.test(v) || 'Valid hostname is required',
                    assetRules: v => /^[1-9]\d{5}$/.test(v) || 'Valid asset number is required',
                    datacenterRules: v => /^(?!\s*$).+/.test(v) || 'Datacenter is required',
                    rackRules: v => /^(?!\s*$).+/.test(v) || 'Rack is required',
                    rackuRules: v => /^(?!\s*$).+/.test(v) || 'Rack U is required',
                    macAddressRules: v => (/^([0-9A-Fa-f]{2}[\W_]*){5}([0-9A-Fa-f]{2})$/.test(v) || /^$/.test(v)) || 'Invalid MAC Address.'
                },
                valid: true
            }
        },

        async created() {
            this.models = await this.modelRepository.list();
            //this.assets = await this.assetRepository.list();
            this.users = await this.userRepository.list();
            //this.racks = await this.rackRepository.list();
            this.datacenters = await this.datacenterRepository.list();

            for (const model of this.models) {
                model.vendorModelNo = model.vendor + " " + model.modelNumber;
            }

            if (typeof this.id !== 'undefined') {
                this.editedItem = await this.assetRepository.find(this.id);
                /*eslint-disable*/
                console.log(this.editedItem);
                this.selectedModel = await this.modelRepository.find(this.editedItem.modelId);
                this.makeNetworkPorts(this.selectedModel);
                this.makePowerPorts(this.selectedModel);
                this.selectedModelBool = true;
                this.selectedRack = true;
                this.rackSelected();
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
                return arr;
            },
        },
        methods: {
            save() {
                if (this.editedItem.id.length != 0) {

                    for (var j = 0; j < this.editedItem.networkPorts.length; j++) {
                        this.editedItem.networkPorts[j].modelNetworkPortId = this.selectedModel.networkPorts[j].id;
                    }
                    console.log(this.editedItem);
                    this.assetRepository.update(this.editedItem).then(this.close());
                } else {
                    for (var i = 0; i < this.editedItem.networkPorts.length; i++) {
                        this.editedItem.networkPorts[i].modelNetworkPortId = this.selectedModel.networkPorts[i].id;
                    }
                    this.assetRepository.create(this.editedItem).then(this.close());
                }
                this.selectedRack = false;
                this.selectedModelBool = false;
            },
            close() {
                this.$router.push({ name: 'assets' })
                this.selectedRack = false;
                this.selectedModelBool = false;
            },
            async sendNetworkPortRequest() {
                this.networks = await this.datacenterRepository.networkPorts(this.datacenterID);
                for (const network of this.networks) {
                    network.nameRackAssetNum = "Port name: " + network.name +
                        ",  " + "Hostname: " + network.assetHostname;

                }
            },
            async updateRacks() {
                if (this.datacenterID != this.editedItem.datacenterId) {

                    this.datacenterID = this.editedItem.datacenterId;
                    this.racks = await this.rackRepository.list(this.datacenterID);
                    this.sendNetworkPortRequest();
                    return true;
                }
                return false;
            },
            async modelSelected() {
                this.selectedModelBool = true;
                this.selectedModel = await this.modelRepository.find(this.editedItem.modelId);
                this.makeNetworkPorts(this.selectedModel);
                this.makePowerPorts(this.selectedModel);
            },
            async rackSelected() {
                this.selectedRack = true;
                let availablePorts = {};
                availablePorts = await this.rackRepository.getPdus(this.editedItem.rackId);
/*                for (var i = 0; i < availablePorts.length; i++) {
                    availablePorts[i].number = +port.number;
                }*/
/*                availablePorts.sort(a, b => a - b); //sorting port numbers so that they are easier to see in frontend
*/              this.availablePortsInRack = availablePorts;
                console.log(this.availablePortsInRack);
            },
            makeNetworkPorts(model) {
                this.networkPorts = [];
                for (var j = 0; j < model.networkPorts.length; j++) {
                    const portInfo = {
                        name: model.networkPorts[j].name,
                        number: model.networkPorts[j].number,
                    }
                    this.networkPorts[j] = Object.assign({}, portInfo);
                }

                if (this.editedItem.id.length == 0) {
                    var networkPortsArray = new Array();
                    for (j = 0; j < model.networkPorts.length; j++) {
                        const newPortInfo = {
                            id: '',
                            macAddress: '',
                            connectedPortId: null,
                        }
                        networkPortsArray.push(Object.assign({}, newPortInfo));
                    }
                    this.editedItem.networkPorts = networkPortsArray;
                }
            },
            makePowerPorts(model) {

                var numPowerPorts = model.powerPorts;
                
                this.powerPorts = [];
                var i;
                for (i = 0; i < numPowerPorts; i++) {

                    const pduInfo = {
                        pduLocation: true,
                    }
                    this.powerPorts[i] = Object.assign({}, pduInfo);
                }

                if (this.editedItem.id.length == 0) {
                    var powerPortsArray = new Array();
                    for (i = 0; i < numPowerPorts; i++) {
                        const powerPortInfo = {
                            id: '',
                            number: 0,
                            pduPortId: null
                        };
                        powerPortsArray.push(Object.assign({}, powerPortInfo));
                    }
                    this.editedItem.powerPorts = powerPortsArray;
                }
            },
            setLocation(side) {
                this.sidePortIsOn = side;
            }
        },

    }
</script>
