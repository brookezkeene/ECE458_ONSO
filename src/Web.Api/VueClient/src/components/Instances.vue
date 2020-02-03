<template>
  <v-card flat>
    <v-card-title>Instances</v-card-title>
    <v-container>
    <v-card>
        <v-spacer></v-spacer>
        <v-data-table
          :headers="headers"
          :items="instances"
          :search="search"
          multi-sort
          @click:row = "showDetails"
        > 
          <!-- Links to the models -->
          <template v-slot:item.model.id = "{ value }">
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
                                                  :loading="loading"
                                                  :items="instances"
                                                  :search-input.sync="search"
                                                  cache-items
                                                  class="mt-4"
                                                  flat
                                                  hide-no-data
                                                  hide-details
                                                  item-text="model.vendor"
                                                  label="Search"
                                                  single-line
                                                  solo-inverted></v-autocomplete>

                              </v-row>
                          </v-col>
                          <v-spacer></v-spacer>
                          <!-- Custom filters; sorts between rack ranges -->
                          <v-col cols="4">
                              <v-row class="pt-6">
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
                          <v-btn color="primary" dark class="mb-2" v-on="on">Add Instance</v-btn>
                      </template>
                      <instance-edit v-bind:editedItem="editedItem"></instance-edit>'
                      <v-card-actions>
                          <v-spacer></v-spacer>
                          <v-btn color="primary" text @click="close">Cancel</v-btn>
                          <v-btn color="primary" text @click="save">Save</v-btn>
                      </v-card-actions>'
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

  export default {
    components: {
      InstanceEdit,
    },
    inject: ['instanceRepository'],
    data() {
      return {
        // Filter values.
        startRackValue: '',
        endRackValue: '',

        dialog: false,
        instructionsDialog: false,
        loading: true,
        search: '',

        // Table data.
        headers: [
          
          { text: 'Model Link',  value: 'model.id', },
          {  text: 'Model Vendor', value: 'model.vendor',   },
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
        firstInstance: null,
        editedIndex: -1,
        defaultItem: {
          hostname: '',
          rack:'',
          owner:'',
          rackPosition:'',
          comment: ''
        },editedItem: {
          hostname: '',
          rack:'',
          owner:'',
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
        this.firstInstance = await this.instanceRepository.find(1);
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
        showInstructions() {
            this.instructionsDialog = true;
        },
        closeDetail() {
            this.instructionsDialog = false;
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