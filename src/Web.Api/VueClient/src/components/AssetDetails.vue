<template>
    <div v-if="!loading">
        <v-card flat>
            <ChangePlanBar></ChangePlanBar>
            <v-card-title>
                <span class="headline">Asset Details</span>
            </v-card-title>
            <v-card-text>
                <v-row v-if="isDecommissioned" >
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Time Decommissioned</v-label>
                        <v-card-text> {{asset.dateDecommissioned}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Decommissioned By</v-label>
                        <v-card-text> {{asset.decommissioner}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model Vendor</v-label>
                        <v-card-text> {{asset.vendor}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model</v-label>
                        <v-card-text>
                            <router-link :to="{ name: 'model-details', params: { id: asset.modelId } }"> {{ asset.modelNumber }} </router-link>
                        </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Host Name</v-label>
                        <v-card-text> {{asset.hostname}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Data Center</v-label>
                        <v-card-text> {{asset.datacenter}} </v-card-text>
                    </v-col>
                    <v-col>
                        <v-label>Location</v-label>
                        <v-card-text v-if="!isBlade"> Rack {{asset.rack}}, Rack Position {{asset.rackPosition}} </v-card-text>
                        <v-card-text v-else> Chassis {{asset.chassisId}}, Slot {{asset.chassisSlot}} </v-card-text>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Asset Number</v-label>
                        <v-card-text> {{asset.assetNumber}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Owner Username</v-label>
                        <v-card-text v-if="ownerPresent"> {{asset.owner}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Comment</v-label>
                        <v-textarea :value="asset.comment" disabled>  </v-textarea>
                    </v-col>
                </v-row>
                <!--NEW port detail page so we can exclude altogether for different kinds of assets-->
                <!--TODO: make show connections false for offline assets-->
                <PortDetails v-if="!isBlade"
                             :asset="asset" 
                             :id="id" 
                             :type="type"></PortDetails> 
                <!--Blade Chassis Diagram -->
                <div v-if="isBlade || isChassis">
                    <v-label>Blade Diagram</v-label>
                    <v-card-text>
                        <blade-diagram :type="type"
                                       :chassisId="asset.chassisId"
                                       :assetId="asset.id"
                                       :mountType="mountType"></blade-diagram>
                    </v-card-text>
                    <v-card-text v-if="isBlade">
                        <v-btn color="primary"
                               outlined
                               @click="toChassisDetails">View Blade Chassis Details</v-btn>
                    </v-card-text>
                </div>
            </v-card-text>

            <v-spacer />

            <!--Back button to return to main page-->
            <v-spacer></v-spacer>
            <a href="javascript:history.go(-1)">Go Back</a>

        </v-card>
    </div>

</template>

<style>
    .v-label {
        font-size: 20px;
    }
    .p {
        font-size: 15px;
    }
</style>

<script>
    import ChangePlanBar from "@/components/ChangePlanStatusBar"
    import PortDetails from '@/components/AssetPortDetails'
    import BladeDiagram from '@/components/BladeDiagram'

    export default {
        name: 'asset-details',
        inject: ['assetRepository', 'modelRepository', 'rackRepository'],
        item: null,
        components: {
            ChangePlanBar,
            PortDetails,
            BladeDiagram
        },
        props: {
            id: String,
            cpId: String,
            type: String,
        },
        data() {
            return {
                loading: false,
                asset: {
                    id:'',
                    hostname: '',
                    rack: '',
                    rackPosition: '',
                    owner: '',
                    comment: '',
                    vendor: '',
                    modelNumber: ''
                },
                ownerPresent: true, // in case the asset does not have an owner, don't need null pointer bc not a required field.
                isDecommissioned: false,
                mountType: '',
            };
        },
        computed: {
            isBlade() {
                return this.mountType === 'blade'
            },
            isChassis() {
                return this.mountType === 'chassis'
            }
        },
        created() {
            this.initialize();
        },
        beforeRouteUpdate(to, from, next) {
            /*eslint-disable*/
            this.id = to.params.id;
            this.$route.params.id = to.params.id;
            this.initialize();

            console.log(from);
            next()
        },
        methods: {
            changePlanId() {
                if (this.$store.getters.isChangePlan)
                    return this.$store.getters.changePlan.id;
            },
            async initialize() {
                /*eslint-disable*/
                if (!this.loading) this.loading = true;
                const asset = await this.assetRepository.find(this.id, this.changePlanId())
                asset.powerPorts.forEach(port => port.status = undefined);
                asset.networkPorts.sort((a, b) => a.number - b.number);
                asset.powerPorts.sort((a, b) => a.number - b.number);

                const model = await this.modelRepository.find(asset.modelId);
                this.mountType = model.mountType;
                console.log(this.mountType)

                this.asset = asset;
                
                if (this.asset.owner === undefined) {
                    this.ownerPresent = false;
                }
                if (this.asset.networkPortGraph !== undefined) {
                    this.isDecommissioned = true;
                }

                this.loading = false;
            },
            toChassisDetails() {
                console.log("new route to chassis")
                this.$router.push({ name: 'asset-details', params: { type: this.type, id: this.asset.chassisId } })
            }
        }
    }
</script>