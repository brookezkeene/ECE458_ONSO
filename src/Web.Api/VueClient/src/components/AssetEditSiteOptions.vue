
    <template>

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
    </template>

<script>

    export default {

        name: 'asset-edit-site',
        props: ['editedItem', 'isBlade'],
        inject: ['datacenterRepository', 'rackRepository'],
        data()  {
            return {
                datacenters: [],
                datacenterID: '',
                racks: [],
                rules: {
                    datacenterRules: v => /^(?!\s*$).+/.test(v) || 'Datacenter is required',
                    rackuRules: v => (/^(?!\s*$).+/.test(v) && v > 0 && v < 43) || 'Valid rack U is required',
                    rackRules: v => /^(?!\s*$).+/.test(v) || 'Location is required',
                }

            }
        },
        async created() {
            const getDatacenters = this.$store.getters.isChangePlan
                ? Promise.resolve(this.datacenters.push(this.$store.getters.changePlan.datacenterName))
                : this.datacenterRepository.list().then( datacenters => {
                    this.datacenters = datacenters;
                    this.$emit('getDatacenters', true)
                })
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
        },
        methods: {
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
        }

    }


</script>