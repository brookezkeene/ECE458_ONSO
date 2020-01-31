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
                <rack-form v-bind:editedDimensions="editedDimensions"></rack-form>
                <v-card-actions>
                <v-btn color="primary" text @click="close">Cancel</v-btn>
                <v-btn color="primary" text @click="save">Save</v-btn>
                </v-card-actions>
            </v-card>
            </v-dialog>
        </v-toolbar>
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

export default {
    components: {
      RackForm,
      RackDiagram
    },
    inject: ['rackRepository'],
    data () {
      return {
        dialog: false,
        loading: true,
        search: '',
        headers: [
          { text: 'Display Name', value: 'displayName' },
          { text: 'Username', value: 'username' },
          { text: 'Email', value: 'email' },
        ],
        racks: [],
        firstRack: null,
        editedIndex: -1,
        editedDimensions: {
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
      this.users = await this.userRepository.list();
      this.firstUser = await this.userRepository.find(1);
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
          Object.assign(this.users[this.editedIndex], this.editedItem)
        } else {
          this.users.push(this.editedItem)
        }
        this.close()
      },
    },
}
</script>