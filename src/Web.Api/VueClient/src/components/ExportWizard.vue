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
                        <v-card-text>
                            Download a CSV file matching the specifications outlined in the following
                            <a target="_blank"
                               rel="noopener noreferrer"
                               href="https://drive.google.com/file/d/1UB8J9E_cKlezRtgtk3g10ikO9Iqz5lGN/view?usp=sharing">
                                document
                            </a>.
                        </v-card-text>
       
                    </v-card>
                    <v-btn color="primary"
                           small="true"
                           @click="step = 2">Continue</v-btn>
                </v-stepper-content>

                <v-stepper-content step="2">
                    <v-card flat class="overflow-y-auto" style="height: 300px">

                        <v-card-title>Export</v-card-title>
                        <v-card>
                            <v-container fill-height fluid>
                                <v-card-title class="justify-center">
                                    Exporting .csv file...
                                </v-card-title>
                                <v-progress-circular indeterminate
                                                     color="primary"></v-progress-circular>
                            </v-container>
                        </v-card>
                    </v-card>
                    <v-btn class="mr-4" @click.native="step = 1" small="true">Previous</v-btn>
                    <v-btn color="primary" @click="step = 3" small="true">Continue</v-btn>
                </v-stepper-content>

            </v-stepper-items>
        </v-stepper>
    </div>
</template>

<script>export default {
    name: 'export-wizard',
    inject: ['exportRepository'],
    props: ['exportWizard'],
    data () {
        return {
            loading: false,
            step: 1,
            fileChosen: false,
            warnings: [ "warning1",
                        "warning2",
                        "warning3",
                        "warning4",
                        "warning5",
            ],   // array to store all warnings, empty if none
            errors: [],     // array to store all errors, empty if none
            headers: [
                { text: 'Record', value: '' },
                { text: 'Status', value: '' },
            ],
            records: [],
            numRecords: 0,
            exportRaw: '',
        };
    },
    methods: {
        async fileDownload() { // not working
            this.exportRaw = await this.exportRepository.exportModel('');   
        },
        // API calls to get warnings and errors and records/statuses
        close() {
            alert('Data Successfully Imported.');
            this.$emit('close-file-chooser');
            this.step = 1;
        }
    }
}</script>