<!--PUT <p style="page-break-before: always;">&nbsp;</p> 
BEFORE AND AFTER CALLING THIS COMPONENT-->
<template>
    <v-container v-if="!loading">
        <v-row v-for="row in racksByRow" v-bind:key="row.rowLetter">
            <v-col v-for="rack in row.racks" v-bind:key="rack.address">
                <table id="table">
                    <!--Labeling the titles of the table-->
                    <thead>
                        <tr>
                            <th v-bind:style="borderColor">U</th>
                            <th v-bind:style="borderColor">{{rack.address}}</th>
                            <th v-bind:style="borderColor">U</th>
                        </tr>
                    </thead>
                    <!--Filling each cell in the body of the table-->
                    <tbody>
                        <tr v-for="slot in rack.slots" v-bind:key="slot.rackU">
                            <td style="color: white; backgroundColor: black;">
                                {{ slot.rackU }}
                            </td>
                            <td v-bind:style="slot.style">
                                {{ slot.value }}
                            </td>
                            <td style="color: white; backgroundColor: black;">
                                {{ slot.rackU }}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    import rowsOfRacks from '@/repositories/mock-racks-by-rows';
  export default {
    inject: ['rackRepository'],
    el: '#secondTable',
    data () {
        return {
            racks: [],
            racksByRow: rowsOfRacks,
            loading: true,
            search: '',
            firstUser: null,
            borderColor: {color: 'white', backgroundColor: 'black'},
            rows: [],
            styleObject: {
            backgroundColor: 'red',
            fontSize: '13px'      
        }
    }},
    async created () {
        this.fetchRacks();
        //this.createRows();
        //this.fillRows();
    },
    methods: {
        fetchRacks() {
            const start = this.$route.query.start;
            const end = this.$route.query.end;
            this.rackRepository.findInRange(start, end).then((response) => {
                this.racks = response;
                this.loading = false;
            });
        },
        createRows () {
            var i;
            for(i = 0; i < 42; i++){
                var u = 42-i;
                var row = { rackU: u, model: '',
                            style: {color: 'black', backgroundColor: 'green'},
                            numStyle: {color: 'white', backgroundColor: 'black'}};
                row.style.backgroundColor = 'white';
                this.rows.push(row);
            }
        },
        fillRows(){
            var i;
            var instances_length = Object.keys(this.instances).length;
            for(i = 0; i < instances_length; i++) {
                //getting root position and color of the current instance
                var rackU = 42 - this.instances[i].rackPosition;              
                var color = this.instances[i].model.displayColor;

                //changing bottom rackU's name and color 
                this.rows[rackU].style.backgroundColor = color;
                this.rows[rackU].model = this.instances[i].model.vendor;

                //going through the second for loop to fill up the remaining rackU's
                //which the instance fills 
                var j;
                var model_height = this.instances[i].model.height
                for(j = 1; j < model_height; j++) {
                    var position = rackU - j;
                    this.rows[position].style.backgroundColor = color;
                }  
            }          
        }
    }    
}
</script>