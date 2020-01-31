<!--PUT <p style="page-break-before: always;">&nbsp;</p> 
BEFORE AND AFTER CALLING THIS COMPONENT-->
<template>
  <v-card>  
    <table id="table">
        <!--Labeling the titles of the table-->
        <thead>
            <tr>
            <th v-bind:style="borderColor">{{"U"}}</th>
            <th v-bind:style="borderColor">{{"Rack Instance: B12"}}</th>
            <th v-bind:style="borderColor">{{"U"}}</th>
            </tr>
        </thead>
        <!--Filling each cell in the body of the table-->
        <tbody>
            <tr v-for="row in rows" v-bind:key="row">
                <td v-bind:style="row.numStyle">
                    {{row.rackU}} </td>
                <td v-bind:style="row.style">
                    {{row.model}}
                </td>
                <td v-bind:style="row.numStyle"> 
                    {{row.rackU}} </td>
            </tr>
        </tbody>
    </table>
  </v-card>
</template>

<script>
  export default {

    inject: ['instanceRepository'],
    el: '#secondTable',
    data () {
        return{
        instances: [],
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
        this.instances = await this.instanceRepository.list();
        this.firstUser = await this.instanceRepository.find(1);
        this.loading = false;
        this.createRows();
        this.fillRows();
    },

    methods: {
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