<template>
    <v-container align="center" justify="center">
        <v-card>
            <v-card-title>Rack Range Selection</v-card-title>

            <v-card-text class="justify-center">
                <form>
                    <v-select v-model="selectedDatacenter"
                              :items="datacenters"
                              item-text="name"
                              item-value=""
                              :return-object="false"
                              label="Datacenter"
                              placeholder="Select a datacenter or all datacenters"
                              clearable>
                    </v-select>
                    <v-text-field v-model="range.start" 
                                  label="Start"
                                  placeholder="Enter the start of an en masse range (i.e. A1)"></v-text-field>
                    <v-text-field v-model="range.end" 
                                  label="End"
                                  placeholder="Enter the end of an en masse range (i.e. Z20)"></v-text-field>

                    <v-row>
                        <v-btn class="mr-4 ml-4" color="primary" @click="viewDiagram">View Diagrams</v-btn>
                        <v-spacer></v-spacer>
                        <v-btn v-if="admin" class="mr-4" color="primary" @click="createInRange">Add Racks</v-btn>
                        <v-btn v-if="admin" class="mr-4" color="primary" @click="deleteInRange">Delete Racks</v-btn>
                    </v-row>
                    
                </form>
            </v-card-text>
        </v-card>
    </v-container>
</template>

<script>
import Auth from "../auth"

export default {
    name: 'rack-form',
    inject: ['rackRepository', 'datacenterRepository'],
    data: () => ({
        loading: true,
        range: {
            start: '',
            end: ''
        },
        datacenters: [],
        selectedDatacenter: '',
    }),
    computed: {
        admin() {
            return Auth.isAdmin()
        },
    },
    async created () {
            this.initialize()
    },
    methods: {
        async initialize() {
            this.datacenters = await this.datacenterRepository.list(); // mock API returns even "All Datacenters", but real response shouldn't
            this.loading = false;
         },
        viewDiagram() {
            this.$router.push({ name: "RackDiagram", query: { start: this.range.start, end: this.range.end } }); // include datacenter in this query
        },
        async createInRange() {
            await this.rackRepository.createInRange(this.range.start, this.range.end) // include datacenter in this query
                .then(() => {
                    // indicate success
                })
                .catch(() => {
                    // indicate failure
                });

            // This might slow things down if we have a lot of racks to get from the backend !!!
            this.$emit('update-table'); // trigger event to update rack-table
        },
        async deleteInRange() {
            await this.rackRepository.deleteInRange(this.range.start, this.range.end) // include datacenter in this query
                .then(() => {
                    // indicate success
                })
                .catch(() => {
                    // indicate failure
                });

            // This might slow things down if we have a lot of racks to get from the backend !!!
            this.$emit('update-table'); // trigger event to update rack-table
        }
    }
}
</script>