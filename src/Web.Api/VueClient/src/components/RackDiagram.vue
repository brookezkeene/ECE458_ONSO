<style scoped>
    @media print {
        @page {
            size: A4 landscape;
        }
        body {
            margin: 0;
            color: #000;
            background-color: #fff;
            -webkit-print-color-adjust: exact !important;
        }
    }

    .diagram
    {
        break-before: always;
        break-inside: avoid;
    }
    
</style>


<template>
    <div media="print">

    <v-container v-if="!loading">
        <v-row v-for="row in racksByRow" v-bind:key="row.rowLetter" class="diagram">
            <v-col v-for="rack in row.racks" v-bind:key="rack.address">
                <table id="table">
                    <thead>
                        <tr>
                            <th class="rack"></th>
                            <th class="rack">{{rack.address}}</th>
                            <th class="rack"></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="slot in rack.slots" v-bind:key="slot.rackU">
                            <td class="rack">
                                {{ slot.rackU }}
                            </td>
                            <td v-bind:style="slot.style">
                                <div style="slot.style">
                                    <v-btn v-if="!slot.value.text==''"
                                           :color="slot.style.backgroundColor"
                                           :depressed=true
                                           :tile=true
                                           :to="{ name: 'asset-details', params: { id: slot.value.id } }">
                                        {{ slot.value.text }}
                                    </v-btn>
                                    <v-btn v-else
                                           :class="{'disable-events':slot.value.text==''}"
                                           :color="slot.style.backgroundColor"
                                           :depressed=true
                                           :disabled=true
                                           :tile=true>
                                        {{ slot.value }}
                                    </v-btn>
                                </div>
                            </td>
                            <td class="rack">
                                {{ slot.rackU }}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </v-col>
        </v-row>
    </v-container>
    </div>
</template>

<style scoped>
    .rack {
        color: white;
        background-color: black;
    }
    .rack-bottom {
        background-color: black;
    }
    table {
        font-size: 10px;
        border-collapse: collapse;
    }
    .v-btn {
        width: 200px;
        max-height: 15px;
    }
    .v-btn[disabled] {
        opacity:.05;
        width: 200px;
        max-height: 15px;
    }
</style>

<script>
    //import rowsOfRacks from '@/repositories/mock/mock-racks-by-rows';
    import rackDiagram from '@/rackDiagram';
    export default {
        inject: ['rackRepository', 'datacenterRepository'],
    data () {
        return {
            datacenters: [],
            racks: [],
            racksByRow: [],
            loading: true,
        }
    },
    async created () {
        this.fetchRacks();
    },
    methods: {
        async fetchRacks() {
        /* eslint-disable no-unused-vars, no-console */
            var datacenter = this.$route.query.datacenter;
            this.datacenters = await this.datacenterRepository.list();
            var foundDatacenter = this.datacenters.find(o => o.description === datacenter);

            const start = this.$route.query.start;
            const end = this.$route.query.end;
            var id;
            if (foundDatacenter.id === 'undefined') {
                id = null;
            } else {
                id = foundDatacenter.id;
            }
            
            rackDiagram.createRacksByRows(start, end, id).then((response) => {
                this.racksByRow = response;
            });
            console.log(this.racksByRow);
            this.loading = false;
        }
    }    
}
</script>