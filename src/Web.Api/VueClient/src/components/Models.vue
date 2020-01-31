<template>
  <v-card>
    <v-data-table
      :headers="headers"
      :items="models"
      :search="search"
      >
        <template v-slot:top>

        <v-toolbar flat color="white">
          <v-toolbar-title>Models</v-toolbar-title>
          <v-divider
            class="mx-4"
            inset
            vertical
          ></v-divider>
          <v-spacer></v-spacer>

          <v-autocomplete
            append-icon = "mdi-magnify"
            :loading="loading"
            :items="models"
            :search-input.sync="search"
            cache-items
            class="mx-4"
            flat
            hide-no-data
            hide-details
            item-text="vendor"
            label="Search"
            single-line
            solo-inverted
          ></v-autocomplete>

          <v-dialog v-model="dialog" max-width="500px">
              <template v-slot:activator="{ on }">
                <v-btn color = "primary" class="mb-2" v-on="on">Add Model</v-btn>
              </template>
            <v-card>
              <model-edit v-bind:editedItem="editedItem" v-bind:models="models"></model-edit>          
              <v-card-actions>
                <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" text @click="close">Cancel</v-btn>
                  <v-btn color="blue darken-1" text @click="save">Save</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>

        <div class="text-center">
          <v-dialog v-model="detailsDialog" width="500">
            <v-card>
              <v-card-text>
                <model-details v-bind:id="detailItem.id"></model-details>
              </v-card-text>
              <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="orange darken-1" text @click="closeDetail">Close</v-btn>
              </v-card-actions>
            </v-card>
            </v-dialog>
         </div>

      </template>

      <template v-slot:item.coloricon="{ item }">
        <v-icon
          class="mr-2"
          :color=item.displayColor>
          mdi-circle
        </v-icon>
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

import ModelDetails from "./ModelDetails"
import ModelEdit from "./ModelEdit"

  export default {
    components: {
      ModelDetails,
      ModelEdit,
    },
    inject: ['modelRepository'],
    data: () => ({
        dialog: false,
        detailsDialog: false,
        loading: true,
        search: '',
        headers: [
          { text: 'Vendor', 
          align: 'left',
          value: 'vendor' },
          { text: 'Model Number', value: 'modelNumber' },
          { text: 'Height', value: 'height' },
          { text: 'Display Color', value: 'coloricon', sortable: false },
          { text: 'Ethernet Ports', value: 'ethernetPorts' },
          { text: 'Power Ports', value: 'powerPorts' },
          { text: 'CPU', value: 'cpu' },
          { text: 'Memory', value: 'memory' },
          { text: 'Storage', value: 'storage' },
          { text: 'Comment', value: 'comment' },
          { text: 'Actions', value: 'action', sortable: false },

        ],
        models: [],
        firstModel: null,
        editedIndex: -1,
        editedItem: {
          vendor: '',
          modelNumber: '',
          height: 0,
          displayColor: 0,
          ethernetPorts: 0,
          powerPorts: 0,
          cpu: '',
          memory: 0,
          storage: 0,
          comment: ''
        },
        defaultItem: {
          vendor: '',
          modelNumber: '',
          height: 0,
          displayColor: 0,
          ethernetPorts: 0,
          powerPorts: 0,
          cpu: '',
          memory: 0,
          storage: 0,
          comment: ''
        },
          detailItem : {
          comment: ''
        },
      }),
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
    async created () {
      this.initialize()
    },
    methods: {
      async initialize () {
        this.models = await this.modelRepository.list();
        this.firstModel = await this.modelRepository.find(1);
        this.loading = false;
      },
      editItem (item) {
        this.editedIndex = this.models.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },
      deleteItem (item) {
        const index = this.models.indexOf(item)
        confirm('Are you sure you want to delete this item?') && this.models.splice(index, 1)
      },
      close () {
        this.dialog = false
        setTimeout(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        }, 300)
      
      },
      save () {
        if (this.editedIndex > -1) {
          Object.assign(this.models[this.editedIndex], this.editedItem)
        } else {
          this.models.push(this.editedItem)
        }
        this.close()
      },
      showDetails (item) {
        this.detailItem = Object.assign({}, item);
        this.detailsDialog = true;
      },
      closeDetail () {
        this.detailsDialog = false;
      },
    },
  }
</script>