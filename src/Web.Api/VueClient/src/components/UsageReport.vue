<template>
    <v-card flat>
        <v-card-title>Rack Usage Report</v-card-title>
        <v-expansion-panels multiple :hover=true>
            <v-expansion-panel v-for="(title, index) in titles"
                               :key="title">
                <v-expansion-panel-header>{{title}}</v-expansion-panel-header>
                <v-expansion-panel-content>
                    <v-card flat v-if="index===0">
                        <div align="center">
                            <v-progress-circular :rotate="rotate"
                                                 :size="sizeBig"
                                                 :value="usedSpace"
                                                 :width="widthBig"
                                                 :color="primary">~{{usedSpace}}% of Rackspace Used</v-progress-circular>
                        </div>
                    </v-card>

                    <v-card flat v-if="index===1">
                        <div align="center">
                            <v-progress-circular v-for="(vendor, index) in vendorsP" :key="vendor"
                                                 :rotate="rotate"
                                                 :size="sizeMed"
                                                 :value="vendor"
                                                 :width="widthMed"
                                                 :color="primary">
                                {{index}}
                            </v-progress-circular>
                        </div>

                        <v-simple-table y-slot:default>
                            <template>
                                <thead>
                                    <tr>
                                        <th>Vendor</th>
                                        <th>Rackspace Allocated (in U)</th>
                                        <th>Rackspace Allocated (%)</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr v-for="(vendor, index) in vendors" :key="vendor">
                                        <td>{{index}}</td>
                                        <td>{{vendors[index]}}</td>
                                        <td>{{vendorsP[index]}}</td>
                                    </tr>
                                </tbody>
                            </template>
                        </v-simple-table>          
                    </v-card>

                    <v-card flat v-if="index===2">
                        <div align="center">
                            <v-progress-circular v-for="(model, index) in modelsP" :key="model"
                                                 :rotate="rotate"
                                                 :size="sizeMed"
                                                 :value="model"
                                                 :width="widthMed"
                                                 :color="primary">
                                {{index}}
                            </v-progress-circular>
                        </div>
                        <v-simple-table y-slot:default>
                            <template>
                                <thead>
                                    <tr>
                                        <th>Model</th>
                                        <th>Rackspace Allocated (in U)</th>
                                        <th>Rackspace Allocated (%)</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr v-for="(model, index) in models" :key="model">
                                        <td>{{index}}</td>
                                        <td>{{models[index]}}</td>
                                        <td>{{modelsP[index]}}</td>
                                    </tr>
                                </tbody>
                            </template>
                        </v-simple-table>
                    </v-card>

                    <v-card flat v-if="index===3">
                        <div align="center">
                            <v-progress-circular v-for="(owner, index) in ownersP" :key="owner"
                                                 :rotate="rotate"
                                                 :size="sizeMed"
                                                 :value="owner"
                                                 :width="widthMed"
                                                 :color="primary">
                                {{index}}
                            </v-progress-circular>
                        </div>
                        <v-simple-table y-slot:default>
                            <template>
                                <thead>
                                    <tr>
                                        <th>Owner</th>
                                        <th>Rackspace Allocated (in U)</th>
                                        <th>Rackspace Allocated (%)</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr v-for="(owner, index) in owners" :key="owner">
                                        <td>{{index}}</td>
                                        <td>{{owners[index]}}</td>
                                        <td>{{ownersP[index]}}</td>
                                    </tr>
                                </tbody>
                            </template>
                        </v-simple-table>
                    </v-card>
                </v-expansion-panel-content>
            </v-expansion-panel>
        </v-expansion-panels>
    </v-card>
</template>

<style>
    .v-progress-circular {
        color: #4bbd51;
        margin: 20px;
    }
</style>

<script>
    export default {
        name: 'usage-report',
        inject: ['reportRepository'],
        data() {
            return {
                loading: true,
                titles: ["Free vs. Used Rackspace",
                    "Rackspace Allocated per Vendor",
                    "Rackspace Allocated per Model",
                    "Rackspace Allocated per Owner"],
                window: 0,
                rotate: 270,
                sizeBig: 300,
                sizeMed: 150,
                sizeSm: 100,
                widthBig: 30,
                widthMed: 20,
                widthSm: 15,
                usage: {},
            };
        },
        computed: {
            freeSpace() {
                return this.usage.freeSpace
            },
            usedSpace() {
                return this.usage.usedSpace
            },
            vendors() {
                return this.usage.vendors
            },
            vendorsP() {
                return this.usage.vendorsPercent
            },
            models() {
                return this.usage.models
            },
            modelsP() {
                return this.usage.modelsPercent
            },
            owners() {
                return this.usage.owners
            },
            ownersP() {
                return this.usage.ownersPercent
            },
        },
        async created() {
            this.initialize()
        },
        methods: {
            async initialize() {
                this.usage = await this.reportRepository.generate();
                this.loading = false;
            },
        }
    }
</script>