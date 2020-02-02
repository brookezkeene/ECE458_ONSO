<template>
    <v-card>
        <v-toolbar flat color="white">
            <v-toolbar-title>Racks</v-toolbar-title>
            <v-divider
                class="mx-4"
                inset
                vertical
            ></v-divider>
            <v-spacer></v-spacer>

            <v-dialog v-model="dialog" max-width="500px">
            <template v-slot:activator="{ on }">
                <v-btn color="primary" dark class="mb-2" v-on="on">Add Racks</v-btn>
            </template>
            <v-card>
                <rack-form v-bind:editedItem="editedItem"></rack-form>
                <v-card-actions>
                <v-btn color="primary" text @click="close">Cancel</v-btn>
                <v-btn color="primary" text @click="save">Save</v-btn>
                </v-card-actions>
            </v-card>
            </v-dialog>
        </v-toolbar>
        <v-row align="center" justify="center">
            <rack-table v-bind:racks="racks" v-bind:editedItem="editedItem"></rack-table>
        </v-row>
        <v-row>
            <v-col>
                <rack-diagram v-bind:racks="racks"></rack-diagram>
            </v-col>
        </v-row>
    </v-card>
</template>

<script>
import RackForm from "./RackForm"
import RackDiagram from "./RackDiagram"
import RackTable from "./RackTable"

export default {
    components: {
      RackForm,
      RackDiagram,
      RackTable
    },
    inject: ['rackRepository'],
    data () {
      return {
        dialog: false,
        loading: true,
        racks: [],
        firstRack: null,
        editedIndex: -1,
        editedItem: {
            rowStart: '',
            rowEnd: '',
            rackStart: '',
            rackEnd: ''
        },
      }
    },

    watch: {
      dialog (val) {
        val || this.close()
      },
    },

    async created() {
      this.racks = await this.rackRepository.list();
      this.firstRack = await this.rackRepository.find(1);
      this.loading = false;
    },

    methods: {
      close () {
        this.dialog = false
        setTimeout(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        }, 300)
      },

      save () {
          if (this.editedIndex > -1) {
              Object.assign(this.racks[this.editedIndex], this.editedItem)
          } else {
              var i, j;
              for (i = this.editedItem.rowStart.charCodeAt(0); i <= this.editedItem.rowEnd.charCodeAt(0); i++) {
                  for (j = this.editedItem.rackStart; j <= this.editedItem.rackEnd; j++) {
                      var newItem = {
                          "id": 0,
                          "address": String.fromCharCode(i) + j,
                          "position": {
                              "row": String.fromCharCode(i),
                              "column": j,
                          }};

                      this.racks.push(newItem)
                  }
              }
        }
        this.close()
      },
    },
}
</script>