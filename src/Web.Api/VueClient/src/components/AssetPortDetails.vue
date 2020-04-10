<template>
    <v-container>
        <v-row>
            <v-col cols="12" sm="6" md="4">
                <!--MAC Addresses-->
                <v-label>MAC Addresses</v-label>
                <v-card flat class="overflow-y-auto">
                    <v-card flat outlined class="overflow-y-auto" max-height="300px">
                        <v-simple-table dense>
                            <template v-slot:default>
                                <thead>
                                    <tr>
                                        <th class="text-left">Name</th>
                                        <th class="text-left">MAC</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="port in asset.networkPorts" :key="port.id">
                                        <td>{{ port.name }}</td>
                                        <td>{{ port.macAddress }}</td>
                                    </tr>
                                </tbody>
                            </template>
                        </v-simple-table>
                    </v-card>
                </v-card>
            </v-col>
            <v-col v-if="showConnections" cols="12" sm="6" md="4">
                <!--network port connections-->
                <v-label>Network Port Connections</v-label>
                <v-card flat class="overflow-y-auto">
                    <v-card flat outlined class="overflow-y-auto" max-height="300px">
                        <v-simple-table dense>
                            <template v-slot:default>
                                <thead>
                                    <tr>
                                        <th class="text-left">Name</th>
                                        <th class="text-left">Hostname</th>
                                        <th class="text-left">Name</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="port in asset.networkPorts" :key="port.id">
                                        <td>{{ port.name }}</td>
                                        <td>{{ port.connectedPort && port.connectedPort.hostname }}</td>
                                        <td>{{ port.connectedPort && port.connectedPort.name }}</td>
                                    </tr>
                                </tbody>
                            </template>
                        </v-simple-table>
                    </v-card>
                </v-card>

                <v-btn small class="mt-4" color="primary" outlined v-if="!showNeighborhood" @click="showNeighborhood = true">View Network Neighborhood</v-btn>
                <v-btn small class="mt-4" color="primary" outlined v-if="showNeighborhood" @click="showNeighborhood = false">Hide Network Neighborhood</v-btn>
            </v-col>
            <v-col v-if="showConnections" cols="12" sm="6" md="4">
                <!--power port connections-->
                <v-label>Power Port Connections</v-label>
                <v-card flat class="overflow-y-auto">
                    <v-card flat outlined class="overflow-y-auto">
                        <v-simple-table dense>
                            <template v-slot:default>
                                <thead>
                                    <tr>
                                        <th class="text-left">#</th>
                                        <th class="text-left">Port</th>
                                        <th class="text-left">Status</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="port in asset.powerPorts" :key="port.id">
                                        <td>{{ port.number }}</td>
                                        <td>{{ port.pduPort }}</td>
                                        <td>{{ port.status }}</td>
                                    </tr>
                                </tbody>
                            </template>
                        </v-simple-table>
                    </v-card>
                </v-card>

                <v-container v-if="!changePlanId()">
                    <v-btn small class="mt-4" color="primary" outlined :disabled="viewPowerPorts" @click="showNames">View Power Port Status</v-btn>
                </v-container>
            </v-col>
        </v-row>
        <v-row v-if="showNeighborhood">
            <network-neighborhood v-bind:id="id" @click="nodeClicked"></network-neighborhood>
        </v-row>
    </v-container>

</template>

<script>

    import NetworkNeighborhood from "./NetworkNeighborhood"

    export default {
        name: 'asset-port-details',
        inject: ['assetRepository'],
        props: ['asset', 'id', 'type'],
        components: {
            NetworkNeighborhood
        },
        data() {
            return {
                viewNames: false,
                viewPowerPorts: false,
                powerPorts: {},
                showNeighborhood: false
            };
        },
        computed: {
            showConnections() {
                if (this.type === 'offline') {
                    return false
                } else {
                    return true
                }
            }
        },
        methods: {
            changePlanId() {
                if (this.$store.getters.isChangePlan)
                    return this.$store.getters.changePlan.id;
            },
            async fetchPowerPortIds() {
                var powerPortStates = [];
                /*eslint-disable*/
                powerPortStates = await this.assetRepository.getPowerPortState(this.asset.id);
                console.log(powerPortStates);
                for (var i = 0; i<powerPortStates.powerPorts.length; i++) {
                    if (powerPortStates.powerPorts[i].status=='0') {
                        powerPortStates.powerPorts[i].status = 'on';
                    } else {
                        powerPortStates.powerPorts[i].status = 'off'
                    }
                }
                this.asset.powerPorts.forEach(port => {
                    var maybeState = powerPortStates.powerPorts.find(o => o.port === port.pduPort);
                    port.status = maybeState && maybeState.status;
                })
                return powerPortStates;
            },
            async showNames() {
                this.powerPorts = await this.fetchPowerPortIds();
                console.log(this.powerPorts);
                this.viewPowerPorts = true;
            },
            hideNames() {
                this.viewPowerPorts = false;
            },
            nodeClicked(e) {
                /* eslint-disable no-unused-vars, no-console */
                console.log('clicked');
                this.$router.push({ name: 'asset-details', params: { id: e } });
            },
        }
    }
</script>
