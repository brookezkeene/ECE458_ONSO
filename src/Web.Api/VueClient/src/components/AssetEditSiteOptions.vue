
    <template>
        <v-row>
            <v-col v-if="assetType ==='active'" cols="12" sm="6" md="4">
                <v-autocomplete v-model="editedItem.datacenterId"
                                label="Datacenter"
                                placeholder="Please select an existing datacenter"
                                :rules="[rules.datacenterRules]"
                                :items="filteredDatacenters"
                                item-text="name"
                                item-value="id"
                                @change="updateRacks">
                </v-autocomplete>
            </v-col>
            <!--todo: replace this data with offline storage data-->
            <v-col v-else cols="12" sm="6" md="4">
                <v-autocomplete v-model="editedItem.datacenterId"
                                label="Offline Storage"
                                placeholder="Please select an existing offline storage site"
                                :rules="[rules.offlineRules]"
                                :items="filteredDatacenters"
                                item-text="name"
                                item-value="id">
                </v-autocomplete>
            </v-col>
            <!-- Will need to update to show only racks from the selected datacenter -->
            <v-col v-if="!isBlade && assetType ==='active'"
                   cols="12" sm="6" md="4">
                <v-autocomplete v-if="!editedItem.datacenterId.length==0 && updateRacks()"
                                v-model="editedItem.rackId"
                                label="Rack"
                                placeholder="Please select a rack"
                                :rules="[rules.rackRules]"
                                :items="racks"
                                item-text="address"
                                item-value="id"
                                @change="rackSelected">
                </v-autocomplete>
            </v-col>
            <v-col v-if="!isBlade && assetType ==='active'"
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
            <v-col v-if="isBlade"
                   cols="12" sm="6" md="4">
                <v-autocomplete v-if="!editedItem.datacenterId.length==0 && updateRacks()"
                                v-model="editedItem.chassisId"
                                label="Blade Chassis"
                                placeholder="Please select a blade chassis for this blade"
                                :items="chassis"
                                item-text="hostname"
                                item-value="id"
                                :rules="[rules.rackRules]"
                                @change="rackSelected">
                </v-autocomplete>
            </v-col>
            <!-- Will need to update to show only slots in the selected blade chassis -->
            <v-col v-if="isBlade"
                   cols="12" sm="6" md="4">
                <v-text-field v-if="!editedItem.datacenterId.length==0 && updateRacks()"
                              v-model="editedItem.chassisSlot"
                              label="Slot"
                              placeholder="Please enter a slot for this blade"
                              type="number"
                              :rules="[rules.slotRules]">
                </v-text-field>
            </v-col>
        </v-row>
    </template>

<script>

    export default {

        name: 'asset-edit-site',
        props: ['editedItem', 'isBlade', 'isOffline', 'type'],
        inject: ['datacenterRepository', 'rackRepository'],
        data()  {
            return {
                datacenters: [],
                datacenterID: '',
                racks: [],
                chassis: [],
                rules: {
                    offlineRules: v => /^(?!\s*$).+/.test(v) || 'Offline storage site is required',
                    datacenterRules: v => /^(?!\s*$).+/.test(v) || 'Datacenter is required',
                    rackuRules: v => (/^(?!\s*$).+/.test(v) && v > 0 && v < 43) || 'Valid rack U is required',
                    rackRules: v => /^(?!\s*$).+/.test(v) || 'Location is required',
                    slotRules: v => (/^(?!\s*$).+/.test(v) && v > 0 && v < 15) || 'Valid slot is required',
                }

            }
        },
        async created() {
            // Getting datacenters or offline assets
            /*eslint-disable*/
            if (this.assetType = "offline") {
                this.$store.getters.isChangePlan
                ? Promise.resolve(this.datacenters.push(this.$store.getters.changePlan.datacenterName)) // todo: this will change to getting from the offline endpoint
                : this.datacenterRepository.list().then( datacenters => {
                    this.datacenters = datacenters;
                    this.$emit('getDatacenters', true)
                })
            } else {
                this.$store.getters.isChangePlan
                ? Promise.resolve(this.datacenters.push(this.$store.getters.changePlan.datacenterName))
                : this.datacenterRepository.list().then( datacenters => {
                    this.datacenters = datacenters;
                    this.$emit('getDatacenters', true)
                })
            }
            if (this.$store.getters.isChangePlan) {
                this.datacenterID = this.$store.getters.changePlan.datacenterId
                //this.racks = await this.rackRepository.list(this.datacenterID);               
                //this.chassis = await this.datacenterRepository.chassis(this.datacenterID, this.$store.getters.changePlan.id);
                
            }


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
            assetType() {
                return this.type
            }
        },
        methods: {
            async updateRacks() {
                if (this.datacenterID != this.editedItem.datacenterId) {
                    this.datacenterID = this.editedItem.datacenterId;

                    
                    if (this.$store.getters.isChangePlan && !this.isBlade) {
                        this.racks = await this.rackRepository.list(this.$store.getters.changePlan.datacenterId);
                    }
                    else if (!this.isBlade) {
                        this.racks = await this.rackRepository.list(this.datacenterID);
                        this.$emit('selectedDatacenter');
                        return true;
                    }
                    else if (this.$store.getters.isChangePlan) {
                        this.chassis = await this.datacenterRepository.chassis(this.$store.getters.changePlan.datacenterId, this.$store.getters.changePlan.id);
                    }
                    else {
                        // for blades, get all blade chassis in the datacenter
                        this.chassis = await this.datacenterRepository.chassis(this.datacenterID);
                        
                        return true;
                    }
                }
                return false;
            },

            async rackSelected() {
                this.selectedRack = true;

                if (!this.isBlade) {
                    let availablePorts = {};
                    availablePorts = await this.rackRepository.getPdus(this.editedItem.rackId);
    /*                for (var i = 0; i < availablePorts.length; i++) {
                        availablePorts[i].number = +port.number;
                    }*/
    /*                availablePorts.sort(a, b => a - b); //sorting port numbers so that they are easier to see in frontend
    */              this.availablePortsInRack = availablePorts;
                }
                else {
                    var searchChassis = this.chassis.find(o => o.id === this.editedItem.chassisId);
                    this.editedItem.rackId = searchChassis.rackId;
                }
                
                this.$emit('selectedRack', this.selectedRack);
            },
        }

    }


</script>