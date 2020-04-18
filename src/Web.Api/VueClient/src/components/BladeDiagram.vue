<template>
    <v-card flat>
        <div>
            <table id="table">
                <thead>
                    <tr>
                        <th class="frame"></th>
                        <th class="frame">1</th>
                        <th class="frame">2</th>
                        <th class="frame">3</th>
                        <th class="frame">4</th>
                        <th class="frame">5</th>
                        <th class="frame">6</th>
                        <th class="frame">7</th>
                        <th class="frame">8</th>
                        <th class="frame">9</th>
                        <th class="frame">10</th>
                        <th class="frame">11</th>
                        <th class="frame">12</th>
                        <th class="frame">13</th>
                        <th class="frame">14</th>
                        <th class="frame"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="i in 2" :key="i">
                        <td class="frame"></td>
                        <td v-for="(blade, index) in blades"
                            :key="index"
                            class="blade">
                            <div v-if="i==1">
                                <v-tooltip top>
                                    <template v-slot:activator="{ on }">
                                        <v-btn v-if="blade.value"
                                               class="blade-btn"
                                               :style="blade.style"
                                               height="250px"
                                               :depressed=true
                                               :tile=true
                                               :to="{ name: 'asset-details', params: { type: type, id: blade.id } }"
                                               v-on="on">
                                            {{ blade.text }}
                                        </v-btn>
                                        <v-btn v-else
                                               class="blade-btn"
                                               color="white"
                                               height="250px"
                                               :depressed=true
                                               :disabled=true
                                               :tile=true>
                                        </v-btn>
                                    </template>
                                    <span>Go to {{blade.text}} details</span>
                                </v-tooltip>
                            </div>
                            <div v-else
                                 class="frame">
                                <v-btn color="black"
                                       width="75px"
                                       :depressed=true
                                       :tile=true
                                       ></v-btn>
                            </div>
                        </td>
                        <td class="frame"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </v-card>
</template>

<style>
    table {
        border-collapse: collapse;
    }
    .frame {
        color: white;
        background-color: black;
        width: 25px;
    }
    .blade {
        width: 25px;
    }
    .blade-btn {
        writing-mode: vertical-lr; 
        transform: rotate(180deg);
    }
</style>

<script>
    /*eslint-disable*/
    import bladeDiagram from '@/bladeDiagram';
    export default {
        name: 'blade-diagram',
        inject: ['assetRepository'],
        props: {
            type: String,
            chassisId: String,
            assetId: String,
            mountType: String,
        },
        data () {
            return {
                blades: [],
                loading: true,
            }
        },
        computed: {
            isBlade() {
                return this.mountType === 'blade'
            }
        },
        async created() {
            console.log("created")
            console.log(this.mountType)
            this.fetchBlades();
        },
        methods: {
            async fetchBlades() {
                if (this.isBlade) {
                    console.log("fetch blades callded with chassisId " + this.chassisId)
                    bladeDiagram.createSlots(this.chassisId).then((blades) => {
                        this.blades = blades
                        console.log(this.blades)

                    });
                }
                else {
                    console.log("fetch blades callded with assetId " + this.assetId)
                    bladeDiagram.createSlots(this.assetId).then((blades) => {
                        this.blades = blades
                        console.log(this.blades)
                    });
                }

                this.loading = false
            },
        },
        
    }
</script>