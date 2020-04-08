<template>
    <div>
        <v-card flat>
            <changePlanBar></changePlanBar>
            <v-card-title>
                <span class="headline">{{formTitle}}</span>
            </v-card-title>
            <v-card-text v-if="loading">
                <v-container fluid>
                    <v-progress-circular :size="75"
                                         indeterminate>
                    </v-progress-circular>
                </v-container>
            </v-card-text>
            <v-card-text v-if="!loading">
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
                                                                      clearable
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
                                                                        label="Datacenter"
                                                                        placeholder="Please select an existing datacenter"
                                                                        :rules="[rules.datacenterRules]"
                                                                        :items="filteredDatacenters"
                                                                        item-text="name"
                                                                        item-value="id">
                                                        </v-autocomplete>
                                                    </v-col>
                                                    <!-- Will need to update to show only racks from the selected datacenter -->
                                                    <v-col v-if="!isBlade"
                                                           cols="12" sm="6" md="4">
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
                                                    <v-col v-if="!isBlade"
                                                           cols="12" sm="6" md="4">
                                                        <v-text-field v-if="!editedItem.datacenterId.length==0 && updateRacks()"
                                                                      v-model.number="editedItem.rackPosition"
                                                                      placeholder="Please enter a rack U for the asset"
                                                                      :rules="[rules.rackuRules]"
                                                                      label="Rack Position"
                                                                      type="number">
                                                        </v-text-field>
                                                    </v-col>

                                                    <!-- Blade Specific Fields (Blade Chassis & Slot) -->
                                                    <!-- TODO: replace edited item fields in v-model to integrate-->
                                                    <v-col v-if="isBlade"
                                                           cols="12" sm="6" md="4">
                                                        <v-autocomplete v-if="!editedItem.datacenterId.length==0 && updateRacks()"
                                                                        v-model="editedItem.rackId"
                                                                        label="Blade Chassis"
                                                                        placeholder="Please select a blade chassis for this blade"
                                                                        :items="racks"
                                                                        item-text="address"
                                                                        item-value="id"
                                                                        :rules="[rules.rackRules]"
                                                                        @change="rackSelected">
                                                        </v-autocomplete>
                                                    </v-col>
                                                    <!-- Will need to update to show only slots in the selected blade chassis -->
                                                    <v-col v-if="isBlade"
                                                           cols="12" sm="6" md="4">
                                                        <v-text-field v-if="!editedItem.datacenterId.length==0 && updateRacks()"
                                                                      v-model="editedItem.rackPosition"
                                                                      label="Slot"
                                                                      placeholder="Please enter a slot for this blade"
                                                                      type="number"
                                                                      :rules="[rules.slotRules]">
                                                        </v-text-field>
                                                    </v-col>
                                                </v-row>
                                            </div>

                                            <div>
                                                <v-row>
                                                    <v-col cols="12" sm="6" md="4">
                                                        <v-autocomplete v-model=editedItem.ownerId
                                                                        :items="users"
                                                                        clearable
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
                                    <!-- Customizable Asset Fields -->
                                    <v-card class="overflow-y-auto"
                                            max-height="500px"
                                            flat
                                            v-if="index===1">

                                    </v-card>
                                    <!-- MAC Addresses and Network Port Connections -->
                                    <v-card class="overflow-y-auto"
                                            max-height="500px"
                                            flat
                                            v-if="index===2">
                                        <div v-if="!isBlade">
                                            <div>
                                                <p v-if="editedItem.networkPorts.length > 0">Enter a MAC Address and a connection for each Network Port below.</p>
                                                <p v-else>No model selected. Please select a model first.</p>
                                            </div>
                                            <v-container fluid
                                                         fill
                                                         v-for="(port, index) in networkPorts" :key="index">
                                                <v-layout align-center
                                                          justify-bottom>
                                                    <v-spacer></v-spacer>
                                                    <v-col cols="12" sm="6" md="2">
                                                        <p>{{port.name}}</p>
                                                    </v-col>
                                                    <v-col cols="12" sm="6" md="4" align-right>
                                                        <v-text-field v-model="editedItem.networkPorts[index].macAddress"
                                                                      placeholder="Please enter a 6-byte hexadecimal string"
                                                                      :rules="[rules.macAddressRules]">
                                                        </v-text-field>
                                                    </v-col>
                                                    <v-col cols="12" sm="6" md="4">
                                                        <v-autocomplete v-model="editedItem.networkPorts[index].connectedPortId"
                                                                        :items="networks"
                                                                        item-text="nameRackAssetNum"
                                                                        item-value="id"
                                                                        label="Connected Network Port"
                                                                        persistent-hint
                                                                        class="overflow-x-auto">
                                                        </v-autocomplete>
                                                    </v-col>
                                                    <v-spacer></v-spacer>
                                                </v-layout>
                                            </v-container>
                                        </div>
                                        <div v-else>
                                            <p>Blades do not have network ports.</p>
                                        </div>
                                    </v-card>
                                    <!-- Power Ports and PDUs -->
                                    <v-card class="overflow-y-auto"
                                            max-height="500px"
                                            flat
                                            v-if="index===3">
                                        <div v-if="!isBlade">
                                            <div>
                                                <p v-if="(selectedRack && selectedModelBool) || (id!=undefined)">Enter the PDU and PDU Number for each Power Port below.</p>
                                                <p v-else>No model or rack selected. Please select a model and a rack first.</p>
                                            </div>
                                            <div v-if="(selectedRack && selectedModelBool) || (id!=undefined)">

                                                <v-container fluid
                                                             fill
                                                             v-for="(port, index) in editedItem.powerPorts" :key="index">
                                                    <v-layout align-center
                                                              justify-bottom>
                                                        <v-spacer></v-spacer>
                                                        <v-spacer></v-spacer>

                                                        <p>Power Port {{index+1}}</p>
                                                        <v-spacer></v-spacer>
                                                        <v-btn-toggle v-model="port.pduLocation"
                                                                      mandatory>
                                                            <v-btn @click="setLocation(port)" value="left">
                                                                Left
                                                            </v-btn>
                                                            <v-btn @click="setLocation(port)" value="right">
                                                                Right
                                                            </v-btn>
                                                        </v-btn-toggle>
                                                        <v-spacer></v-spacer>
                                                        <v-autocomplete v-model="port.pduPortId"
                                                                        :items="availablePortsInRack[port.pduLocation]"
                                                                        item-text="number"
                                                                        item-value="id"
                                                                        typeof="number"
                                                                        clearable
                                                                        placeholder="PDU Number">
                                                        </v-autocomplete>
                                                        <v-spacer></v-spacer>
                                                    </v-layout>
                                                </v-container>
                                            </div>
                                        </div>
                                        <div v-else>
                                            <p>Blades do not have power ports.</p>
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
    /* eslint-disable */
    import changePlanBar from '@/components/ChangePlanStatusBar';

    export default {
        name: 'asset-edit',
        components: {
            changePlanBar,
        },
        inject: ['assetRepository', 'modelRepository', 'userRepository', 'rackRepository', 'datacenterRepository', 'networkRepository', 'powerRepository'],
        props: {
            id: String,
        },
        data() {
            return {
                models: [],
                users: [],
                racks: [],
                networks: [],
                datacenters: [],
                loading: true,
                titles: [
                    "Basic Asset Information",
                    "Customizable Asset Information",
                    "MAC Addresses and Network Port Connections",
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
                    rackPosition: 1,
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
                    assetRules: v => (/^[1-9]\d{5}$/.test(v) || (v || '').length==0) || 'Invalid Asset Number',
                    datacenterRules: v => /^(?!\s*$).+/.test(v) || 'Datacenter is required',
                    rackRules: v => /^(?!\s*$).+/.test(v) || 'Location is required',
                    rackuRules: v => (/^(?!\s*$).+/.test(v) && v > 0 && v < 43) || 'Valid rack U is required',
                    slotRules: v => (/^(?!\s*$).+/.test(v) && v > 0 && v < 15) || 'Valid slot is required',
                    macAddressRules: v => (/^([0-9A-Fa-f]{2}[\W_]*){5}([0-9A-Fa-f]{2})$/.test(v) || /^$/.test(v)) || 'Invalid MAC Address.'
                },
                valid: true
            }
        },

        async created() {
            const getModels = this.modelRepository.list()
                .then(models => {
                    models.forEach(model => model.vendorModelNo = model.vendor + " " + model.modelNumber);
                    this.models = models;
                })

            const getUsers = this.userRepository.list()
                .then(users => this.users = users);

            const getDatacenters = /*this.$store.getters.isChangePlan
                ? Promise.resolve(this.datacenters.push(this.$store.getters.changePlan.datacenterName))
                :*/ this.datacenterRepository.list().then(datacenters => this.datacenters = datacenters);

            const getAsset = typeof this.id === 'undefined' || this.id === 'new'
                ? Promise.resolve()
                : this.assetRepository.find(this.id, this.$store.getters.isChangePlan)
                    .then(asset => this.editedItem = asset)
                    .then(() => this.modelRepository.find(this.editedItem.modelId))
                    .then(model => this.selectedModel = model)
                    .then(() => {
                        this.makeNetworkPorts(this.selectedModel);
                        this.makePowerPorts(this.selectedModel);
                        this.selectedModelBool = true;
                        this.selectedRack = true;
                        this.rackSelected();
                    });

            Promise.all([getModels, getUsers, getDatacenters, getAsset])
                .then(() => this.loading = false);
        },
        computed: {
            datacenterPermissions() {
                return this.$store.getters.hasDatacenters
            },
            filteredDatacenters() {
                if (!this.datacenterPermissions.includes("All Datacenters")) {
                    var newDatacenters = []
                    for (var i = 0; i < this.datacenters.length; i++) {
                        if (this.datacenterPermissions.includes(this.datacenters[i].description)) {
                            newDatacenters.push(this.datacenters[i]);
                        }
                    }
                    return newDatacenters;
                }
                else {
                    return this.datacenters;
                }
            },
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
            isBlade() {
                //return this.editedItem.mountType === 'blade'
                return false // TODO: replace this with line above when integrating with backend
            }
        },
        methods: {
            save() {
               

                // Check if in a change plan context
                if (this.$store.getters.isChangePlan) {
                    this.editedItem.changePlanId = this.$store.getters.changePlan.id;
                    if (this.editedItem.id == "00000000-0000-0000-0000-000000000000" && this.id !== 'undefined') {
                        this.editedItem.id = this.id;

                    }
                }


            
                var promise = typeof this.id !== 'undefined'
                    ? this.assetRepository.update(this.editedItem)
                    : this.assetRepository.create(this.editedItem);
                promise.then(this.close);
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

                    if (!this.isBlade) {
                        this.racks = await this.rackRepository.list(this.datacenterID);
                        this.sendNetworkPortRequest();
                        return true;
                    }
                    else {
                        // TODO: for blades, get all blade chassis in the datacenter and set this equal to racks
                        this.racks = [];
                        return true;
                    }
                }
                return false;
            },
            async modelSelected() {
                this.selectedModelBool = true;
                this.selectedModel = await this.modelRepository.find(this.editedItem.modelId);
                console.log(this.selectedModel);
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
                console.log(model);
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
                            modelNetworkPortId: model.networkPorts[j].id
                        }
                        networkPortsArray.push(Object.assign({}, newPortInfo));
                    }
                    this.editedItem.networkPorts = networkPortsArray;
                }
                console.log(networkPortsArray);

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
            setLocation(port) {
                port.pduPortId = null;
            }
        },

    }
</script>
