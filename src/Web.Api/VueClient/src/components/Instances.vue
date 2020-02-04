<template>
  <v-card flat>
    <v-card-title>Instances</v-card-title>
    <v-container>
    <v-card>
        <v-spacer></v-spacer>
        <v-data-table
          :headers="filteredHeaders"
          :items="instances"
          :search="search"
          multi-sort
          @click:row = "showDetails"
        > 
          <!-- Links to the models -->
          <template v-slot:item.model.vendor = "{ value }">
                <a>{{ value }}</a>     
          </template>

          <template v-slot:top>
              <v-toolbar flat>
                  <!-- ADDED AUTOCOMPLETE TO THE MODEL SEARCH -->
                  <v-container fluid align="left">
                      <v-row>
                          <v-col cols="6">
                              <v-row>
                                  <v-autocomplete prepend-inner-icon="mdi-magnify"
                                                  :items="instances"
                                                  :search-input.sync="search"
                                                  cache-items
                                                  class="mt-3 pt-3"
                                                  flat
                                                  hide-no-data
                                                  hide-details
                                                  item-text="model.vendor"
                                                  label="Search by keyword on model and hostname"
                                                  single-line
                                                  solo-inverted></v-autocomplete>

                              </v-row>
                          </v-col>
                          <v-spacer></v-spacer>
                          <!-- Custom filters; sorts between rack ranges -->
                          <v-col cols="4">
                              <v-row class="mt-4 pt-6">
                                  <v-text-field v-model="startRackValue"
                                                placeholder="Start"
                                                type="text"
                                                label="Rack Range"
                                                style="width:0">
                                  </v-text-field>

                                  <v-text-field v-model="endRackValue"
                                                type="text"
                                                placeholder="End"
                                                style="width:0">
                                  </v-text-field>
                              </v-row>
                          </v-col>
                          <v-spacer></v-spacer>
                      </v-row>
                  </v-container>

                  <v-spacer></v-spacer>

                  <!-- Calls for InstanceDetails and InstanceEdit cards -->
                  <v-dialog v-model="dialog" max-width="500px">
                      <template v-slot:activator="{ on }">
                          <v-btn v-if="admin" color="primary" dark class="mb-2" v-on="on">Add Instance</v-btn>
                      </template>
                      <v-card>
                          <instance-edit v-bind:editedItem="editedItem"></instance-edit>'
                          <v-card-actions>
                              <v-spacer></v-spacer>
                              <v-btn text @click="close">Cancel</v-btn>
                              <v-btn color="primary" text @click="save">Save</v-btn>
                          </v-card-actions>'
                      </v-card>
                  </v-dialog>


                  <v-dialog v-model="instructionsDialog" max-width="550px">
                      <v-card>
                          <v-card-title class="justify-center">
                              Click on row for more information about the instance
                          </v-card-title>
                      </v-card>
                  </v-dialog>

              </v-toolbar>


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
          </template>
        <template v-slot:no-data>
          <v-btn color="primary" @click="initialize">Reset</v-btn>
        </template>
        ></v-data-table>
    </v-card>
    </v-container>
  </v-card>
</template>

<script>
    import InstanceEdit from "./InstanceEdit"
    import Auth from "../auth"

  export default {
    components: {
      InstanceEdit,
    },
    inject: ['instanceRepository','modelRepository'],
    data() {
      return {
        // Filter values.
        startRackValue: '',
        endRackValue: '',
        modelFilterValue: '',

        dialog: false,
        instructionsDialog: false,
        loading: true,
        search: '',

        // Table data.
        headers: [
          

          { text: 'Model Vendor', value: 'model.vendor', filter: this.modelFilter },
          { text: 'Model Number', value: 'model.modelNumber', },
          { text: 'Hostname', value: 'hostname' },
          { text: 'Rack', value: 'rack', filter: this.rackFilter },
          { text: 'Rack U', value: 'rackPosition', },
          { text: 'Owner First Name', value: 'owner.firstName', },
          { text: 'Owner Last Name', value: 'owner.lastName' },
          { text: 'Owner Username', value: 'owner.username' },
          { text: 'Owner Email',  value: 'owner.email'},
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
            firstName:'',
            lastName:'',
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
            firstName:'',
            lastName:'',
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
        deleting: false,
      }},
    computed: {
        formTitle () {
            return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
        },
        admin() {
            return Auth.isAdmin()
        },
        filteredHeaders() {
            return (this.admin) ? this.headers : this.headers.filter(h => h.text !== "Actions")
        },
    },
    watch: {
      dialog (val) {
        val || this.close()
      },
      instructionsDialog (val) {
        val || this.closeDetail()
      },
    },
    async created() {
      this.initialize()
    },
  
    methods: {
      async initialize () {
        this.instances = await this.instanceRepository.list();
        this.models = await this.modelRepository.list();
        this.loading = false;
      },
     
      editItem (item) {
        this.editedIndex = this.instanceRepository.find(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },
      deleteItem (item) {
        this.deleting = true;
        confirm('Are you sure you want to delete this item?') && this.instanceRepository.delete(item)
      },
      close () {
        this.dialog = false
        setTimeout(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        }, 300)
      },
        showInstructions() {
            this.instructionsDialog = true;
        },
        closeDetail() {
            this.instructionsDialog = false;
        },
    
      save () {
        if (this.editedIndex > -1) {
          this.instanceRepository.update(this.editedItem)

        } else {
          this.instanceRepository.create(this.editedItem)
        }
        this.close()
      },
        showDetails(item) {
          if (this.editedIndex === -1 && !this.deleting) {
              this.detailItem = Object.assign({}, item)
              this.$router.push({ name: 'instance-details', params: { detailItem: this.detailItem, id: this.detailItem.id } })
          }
          this.deleting = false;
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