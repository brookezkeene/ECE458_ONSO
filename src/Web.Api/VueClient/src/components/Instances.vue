<template>
  <v-card>
    <v-card-title>
      Instances
      <v-spacer></v-spacer>
      
    </v-card-title>
    <v-data-table
      :headers="headers"
      :items="instances"
      :search="search"
      multi-sort
    > 
      <!-- Links to the models -->
      <template v-slot:item.model.id = "{ value }">
            <a> {{ value }} </a>     
      </template>

    <template v-slot:top>
      
      <!-- ADDED AUTOCOMPLETE TO THE MODEL SEARCH -->
      <v-container fluid>
        <v-row>
            <v-col cols="6">
                <v-row class="pa-6">
                    <v-autocomplete 
                        v-model="modelFilterValue"
                        :items="instances"
                        item-text="model.vendor"
                        placeholder="Start typing to Search for Model"
                        label="Model Search">
                    </v-autocomplete>
                </v-row>
            </v-col>

            <!-- Custom filters; sorts between rack ranges -->
            <v-col cols="6">
                <v-row class="pa-6">
                    <v-text-field 
                      v-model="startRackValue" 
                      placeholder="Start typing to sort start of racks"
                      type="text" 
                      label="Rack Range ">
                    </v-text-field>
                    
                    <v-spacer></v-spacer>
                    
                    <v-text-field 
                      v-model="endRackValue" 
                      type="text" 
                      placeholder="Start typing to search end of racks">
                    </v-text-field>

                </v-row>
             </v-col>
        </v-row>
      </v-container>


        <v-toolbar flat>
          <v-toolbar-title>Table</v-toolbar-title>
            
          <v-spacer></v-spacer>

          <!-- Calls for InstanceDetails and InstanceEdit cards -->
          <v-dialog v-model="dialog" max-width="500px">
              <template v-slot:activator="{ on }">
                  <v-btn color="primary" dark class="mb-2" v-on="on">New Item</v-btn>
              </template>
              <v-card>
                  <instance-edit v-bind:editedItem="editedItem" v-bind:instances="instances" v-bind:models="models"></instance-edit>'
                  <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn color="primary" text @click="close">Cancel</v-btn>
                      <v-btn color="primary" text @click="save">Save</v-btn>
                  </v-card-actions>'
              </v-card>
        </v-dialog>
        </v-toolbar>

          <v-dialog v-model="detailsDialog" width="500px">
            <v-card>
              <instance-details v-bind:id="detailItem.id"></instance-details>
              <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn text @click="closeDetail">Close</v-btn>
              </v-card-actions>
            </v-card>
            </v-dialog>
         

      </template>

      <template v-slot:item.action="{ item }">
        <v-icon
          small
          class="mr-2"
          @click="editItem(item)"
        >
          edit
        </v-icon>
        <v-icon
          small
          class="mr-2"
          @click="deleteItem(item)"
        >
          delete
        </v-icon>
        <v-icon
          small
          class="mr-2"
          @click="showDetails(item)"
        >
          details
        </v-icon>
      </template>
    <template v-slot:no-data>
      <v-btn color="primary" @click="initialize">Reset</v-btn>
    </template>
    ></v-data-table>
  </v-card>
</template>

<script>
  import InstanceEdit from "./InstanceEdit"
  import InstanceDetails from "./InstanceDetails"
  export default {
    components: {
      InstanceEdit,
      InstanceDetails,
    },
    inject: ['instanceRepository','modelRepository'],
    data() {
      return {
        // Filter values.
        startRackValue: '',
        endRackValue: '',
        modelFilterValue: '',

        dialog: false,
        detailsDialog: false,
        loading: true,
        search: '',

        // Table data.
        headers: [
          
          { text: 'Model ID (LINK TO MODEL)',  value: 'model.id', },
          { text: 'Model Vendor', value: 'model.vendor', filter: this.modelFilter },
          { text: 'Model Number', value: 'model.modelNumber', },
          { text: 'Model height', value: 'model.height', },
          { text: 'Display Color', value: 'model.displayColor', },
          { text: 'Ethernet ports', value: 'model.ethernetPorts', },
          { text: 'Power Ports', value: 'model.powerPorts', },
          { text: 'CPU', value: 'model.cpu', },
          { text: 'Memory', value: 'model.memory', },
          { text: 'Storage', value: 'model.storage', }, 


          { text: 'Hostname', value: 'hostname' },
          { 
            text: 'Rack', 
            value: 'rack',
            filter: this.rackFilter 
          },
          { text: 'Rack U', value: 'rackPosition', },
          { text: 'Comment', value: 'comment' },
          { text: 'Actions', value: 'action', sortable: false },
        ],
        instances: [],
        models: [],

        firstInstance: null,
        editedIndex: -1,
        defaultItem: {
          model: {
            id: '',
            vendor: '',
            modelNumber: '',
            height: 0,
            displayColor: '',
            ethernetPorts: 0,
            powerPorts: 0,
            cpu: '',
            memory: '',
            storage: '',
          },
          hostname: '',
          rack:'',
          owner:{
            id:'',
            username:'',
            displayname:'',
            email:'',
          },
          rackPosition:'',
          comment: ''
        },editedItem: {
          model: {
            id: '',
            vendor: '',
            modelNumber: '',
            height: 0,
            displayColor: '',
            ethernetPorts: 0,
            powerPorts: 0,
            cpu: '',
            memory: '',
            storage: '',
          
          },
          hostname: '',
          rack:'',
          owner:{
            id:'',
            username:'',
            displayname:'',
            email:'',
          },
          rackPosition:'',
          comment: ''
        },
        detailItem : {
          hostname:'',
          rack:'',
          rackPosition:'',
          owner:'',
          comment: ''
        },

      }},
      computed: {
      formTitle () {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
      },
    },
    watch: {
      dialog (val) {
        val || this.close()
      },
      detailsDialog (val) {
        val || this.closeDetail()
      },
    },
    async created() {
      this.initialize()
    },
  
    methods: {
      async initialize () {
        this.instances = await this.instanceRepository.list();
        this.firstInstance = await this.instanceRepository.find(1);
        this.models = await this.modelRepository.list();
        this.loading = false;
      },
     
      editItem (item) {
        this.editedIndex = this.instances.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },
      deleteItem (item) {
        const index = this.instances.indexOf(item)
        confirm('Are you sure you want to delete this item?') && this.instances.splice(index, 1)
      },
      close () {
        this.dialog = false
        setTimeout(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        }, 300)
      },
      closeDetail () {
        this.detailsDialog = false
      },
    
      save () {
        if (this.editedIndex > -1) {
          Object.assign(this.instances[this.editedIndex], this.editedItem)
        } else {
          this.instances.push(this.editedItem)
        }
        this.close()
      },
      showDetails (item) {
          this.detailItem = Object.assign({}, item)
          this.$router.push({ name: 'instance-details', params: { detailItem: this.detailItem, id: this.detailItem.id } })
        //this.detailsDialog = true
      },
      /**
        Filters for all the model searches 
       */
      modelFilter(value) {
        if (!this.modelFilterValue) {
          return true;
        }
        return value.toLowerCase().includes(this.modelFilterValue.toLowerCase());
      },


      /**
       * Filter for calories column.
       * @param value Value to be tested; in this case the rack value.
       * @returns {boolean} based on the start and end rack value inputs 
       */
      rackFilter(value) {
        // If this filter has no value we just skip the entire filter.
        if (!this.startRackValue && !this.endRackValue) {
          return true;
        // If only one filter has a value, leans entirely on that one filter
        } else if (!this.endRackValue) {
          return value.toLowerCase() >= this.startRackValue.toLowerCase();
        } else if (!this.startRackValue) {
          return value.toLowerCase() <= this.endRackValue.toLowerCase();
        }  
 
        // Check if the current loop value (The rack value)
        // is between the rack values inputted
        return value.toLowerCase() >= this.startRackValue.toLowerCase()
                && value.toLowerCase() <= this.endRackValue.toLowerCase();
      },
    
    },
  }
</script>