<template>
    <div v-if="!loading">
        <v-stepper v-model="step">
            <v-stepper-header>
                <v-stepper-step step="1" :complete="step > 1">Select Filters</v-stepper-step>
                <v-divider></v-divider>
                <v-stepper-step step="2" :complete="step > 2">Confirm</v-stepper-step>
            </v-stepper-header>

            <v-stepper-items>
                <v-stepper-content step="1">
                    <v-card flat class="overflow-y-auto" style="height: 300px">

                        <v-card-title>Export</v-card-title>
                        <v-card flat>
                            <v-container fill-height fluid>
                                <v-label>
                                    Input a host name to filter result by
                                </v-label>
                                <v-text-field v-model="query.HostName">
                                </v-text-field>
                            </v-container>
                        </v-card>
                    </v-card>
                    <v-btn color="primary" @click="setStep2" small=true>Continue</v-btn>
                </v-stepper-content>

                <v-stepper-content step="2">
                    <v-card flat class="overflow-y-auto" style="height: 300px">
                        <v-card-text>
                            Download a CSV file matching the specifications outlined in the following
                            <a target="_blank"
                               rel="noopener noreferrer"
                               href="https://drive.google.com/file/d/1UB8J9E_cKlezRtgtk3g10ikO9Iqz5lGN/view?usp=sharing">
                                document
                            </a>.
                            <downloadCsv :data="exportRaw"
                                         name="models.csv"
                                         type="csv">
                                <v-spacer></v-spacer>
                                <v-btn>
                                    <b>Download</b>
                                </v-btn>
                            </downloadCsv>
                        </v-card-text>

                    </v-card>
                    <v-btn class="mr-4" @click.native="step = 1" small=true>Previous</v-btn>
                    <v-btn color="primary" @click.prevent="close" small=true>Close</v-btn>
                </v-stepper-content>

            </v-stepper-items>
        </v-stepper>

        <v-dialog v-model="networkErrorDialog">
            <v-card>
                <v-title>No networks found with these filters!</v-title>
                <v-btn @click.native="step = 1">Try Again</v-btn>
            </v-card>
        </v-dialog>
    </div>
</template>

<script>
    export default {
        name: 'export-network-wizard',
        inject: ['exportRepository'],
        props: ['exportWizard'],
        data () {
            return {
                loading: false,
                step: 1,
                fileChosen: false,
                errors: [],     // array to store all errors, empty if none
                headers: [
                    { text: 'Record', value: '' },
                    { text: 'Status', value: '' },
                ],
                records: [],
                numRecords: 0,
                exportRaw: '',
                model_filter: '',
                json_fields: {
                    'Vendor': 'name',
                },
                query: {
                    Search: '',
                    HostName: '',
                    StartRow: '',
                    StartCol: 0,
                    EndRow: '',
                    EndCol: 0,
                },
                rules: {
                    rackRules: v => /[a-z][0-9]+$/.test(v) || 'Network port name cannot contain whitespace'
                },
                networkErrorDialog: false
            };
        },
        methods: {
            async setStep2() {
                this.exportRaw = await this.exportRepository.exportNetwork(this.query);

                /* eslint-disable no-unused-vars, no-console */
                console.log(this.exportRaw.length);
                console.log(this.exportRaw);
                console.log("This is inside of export raw");

                this.step = 2
            },
            close() {
                this.step = 1;
                this.$emit('close-model-export');
            },
            handleError() {
                this.networkErrorDialog = true;
            },
        }
    }
</script>