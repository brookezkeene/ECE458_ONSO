<template>
  <v-card flat>
    <v-card-title>Instances</v-card-title>
    <v-container>
    <v-card>
        <v-spacer></v-spacer>
        <v-data-table :headers="filteredHeaders"
                      :items="instances"
                      :search="search"
                      multi-sort
                      @click:row="showDetails">

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

                    <v-btn v-if="admin" color="primary" dark class="mb-2" @click="addItem">Add Instance</v-btn>

                    <v-dialog v-model="instructionsDialog" max-width="550px">
                        <v-card>
                            <v-card-title class="justify-center">
                                Click on row for more information about the instance
                            </v-card-title>
                        </v-card>
                    </v-dialog>

                </v-toolbar>


            </template>

            <template v-if="admin" v-slot:item.action="{ item }">
                <v-row>
                    <v-icon medium
                            class="mr-2"
                            @click="editItem(item)">mdi-pencil</v-icon>
                    <v-icon medium
                            class="mr-2"
                            @click="deleteItem(item)">mdi-delete</v-icon>
                </v-row>
            </template>

            <template v-slot:no-data>
                <v-btn color="primary" @click="initialize">Refresh</v-btn>
            </template>
            >
        </v-data-table>
    </v-card>
    </v-container>
  </v-card>
</template>

<script>
    import Auth from "../auth"

  export default {
    components: {
    },
    inject: ['instanceRepository','modelRepository'],
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
          

          { text: 'Model Vendor', value: 'model.vendor' },
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
          rackPosition:0,
          comment: ''
        },
        detailItem : {
          hostname:'',
          rack:'',
          rackPosition:0,
          owner:'',
          comment: ''
        },
        deleting: false,
        editing: false,
      }},
    computed: {
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
    
      deleteItem (item) {
        this.deleting = true;
        confirm('Are you sure you want to delete this item?') && this.instanceRepository.delete(item)
                    .then(async () => {
                        await this.initialize();
                    })
        },

        editItem(item) {
            this.$router.push({ name: 'instance-edit', params: { id: item.id } })
        },

        addItem() {
            this.$router.push({ name: 'instance-new' })
        },

        showInstructions() {
            this.instructionsDialog = true;
        },

        closeDetail() {
            this.instructionsDialog = false;
        },

        showDetails(item) {
          if (!this.editing && !this.deleting) {
              this.detailItem = Object.assign({}, item)
              this.$router.push({ name: 'instance-details', params: { detailItem: this.detailItem, id: this.detailItem.id } })
          }
          this.deleting = false;
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
