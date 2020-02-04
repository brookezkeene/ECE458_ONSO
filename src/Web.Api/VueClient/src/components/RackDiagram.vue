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
                                {{ slot.value }}
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
</style>

<script>
    import rowsOfRacks from '@/repositories/mock/mock-racks-by-rows';
    //import rackDiagram from '@/rackDiagram';
    export default {
    inject: ['rackRepository'],
    data () {
        return {
            racks: [],
            racksByRow: rowsOfRacks,
            loading: true,
        }
    },
    async created () {
        this.fetchRacks();
    },
    methods: {
        fetchRacks() {
            const start = this.$route.query.start;
            const end = this.$route.query.end;
            this.rackRepository.findInRange(start, end).then((response) => {
                this.racks = response;
                this.loading = false;
            });
            //this.racksByRow = rackDiagram.createRacksByRows(start, end);
        }
    }    
}
</script>