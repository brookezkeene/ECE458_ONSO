<template>
    <v-container align-center justify-center>
        <v-card class="pa-10">
            <v-card-title>Rack Range Selection</v-card-title>

            <v-card-text class="justify-center">
                <form>
                    <v-select v-model="selectedDatacenter"
                              :items="datacenters"
                              item-text="description"
                              item-value=""
                              :return-object="false"
                              label="Datacenter"
                              placeholder="Select a datacenter or all datacenters">
                    </v-select>
                    <v-text-field v-model="range.start"
                                  label="Start"
                                  placeholder="Enter the start of a range of racks (i.e. A1)"></v-text-field>
                    <v-text-field v-model="range.end"
                                  label="End"
                                  placeholder="Enter the end of a range of racks (i.e. Z20)"></v-text-field>

                    <v-row>
                        <v-btn class="mr-4 ml-4" color="primary" :disabled="startFilled" @click="viewDiagram">View Diagrams</v-btn>
                        <v-spacer></v-spacer>
                        <v-btn v-if="permission" class="mr-4" color="primary" @click="createInRange">Add Racks</v-btn>
                        <v-btn v-if="permission" class="mr-4" color="primary" @click="deleteInRange">Delete Racks</v-btn>
                    </v-row>

                </form>
            </v-card-text>
            <v-snackbar v-model="updateSnackbar.show"
                        :bottom=true
                        class="black--text"
                        :color="updateSnackbar.color"
                        :timeout=5000>
                {{updateSnackbar.message}}
                <v-btn dark
                       class="black--text"
                       text
                       @click="updateSnackbar.show = false">
                    Close
                </v-btn>
            </v-snackbar>
        </v-card>
    </v-container>
</template>

<script>
export default {
    name: 'rack-form',
    inject: ['rackRepository', 'datacenterRepository'],
    data: () => ({
        loading: true,
        updateSnackbar: {
                    show: false,
                    message: '',
                    color: ''
                },
        range: {
            start: '',
            end: ''
        },
        datacenters: [],
        selectedDatacenter: '',
        rules: {
            rackRules: v => /^[a-zA-Z][0-9]+$/.test(v),
        },
    }),
    computed: {
        permission() {
            return this.$store.getters.isAdmin || this.$store.getters.hasAssetPermission
        },
        datacenterPermissions() {
            return this.$store.getters.hasDatacenters
        },
        startFilled() {
            return this.range.start === '';
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
            if (this.validation() != 0) {
                return;
            }
            this.$router.push({ name: "RackDiagram", query: { start: this.range.start, end: this.range.end, datacenter: this.selectedDatacenter } });
        },
        async createInRange() {
            if (this.validation() != 0) {
                return;
            }
            // check if user has permission for given datacenter
            if (this.datacenterPermissions.includes(this.selectedDatacenter)) {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);

                await this.rackRepository.createInRange(this.range.start, this.range.end, searchDatacenter.id)
                    .then(() => {
                        // indicate success
                        this.$emit('update-create'); // trigger event to update rack-table
                    })
                    .catch(() => {
                        // indicate failure
                        this.$emit('error-create');
                    });

                // This might slow things down if we have a lot of racks to get from the backend !!!
                //this.$emit('update-create'); // trigger event to update rack-table
            }
            else {
                this.$emit('error-datacenter');
            }
        },
        async deleteInRange() {
            if (this.validation() != 0) {
                return;
            }
            // check if user has permission for given datacenter
            if (this.datacenterPermissions.includes(this.selectedDatacenter)) {
                var searchDatacenter = this.datacenters.find(o => o.description === this.selectedDatacenter);

                await this.rackRepository.deleteInRange(this.range.start, this.range.end, searchDatacenter.id)
                    .then(() => {
                        // indicate success
                        this.$emit('update-delete'); // trigger event to update rack-table
                    })
                    .catch(() => {
                        // indicate failure
                        this.$emit('error-delete');
                    });

                // This might slow things down if we have a lot of racks to get from the backend !!!
                //this.$emit('update-delete'); // trigger event to update rack-table
            }
            else {
                this.$emit('error-datacenter');
            }
        },
        validation() {
            var count = 0;
            this.updateSnackbar.message = '';
            if (!(/^[a-zA-Z][0-9]+$/.test(this.range.start)) || !(/^[a-zA-Z][0-9]+$/.test(this.range.start))) {
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'red lighten-4';
                this.updateSnackbar.message = this.updateSnackbar.message + 'Rack action failed. Rack name must be a letter followed by a number ';
                count++;
            }
            return count;
        }
    }
}
</script>