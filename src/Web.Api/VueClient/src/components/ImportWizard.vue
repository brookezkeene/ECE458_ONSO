<template>
    <div v-if="!loading">
        <v-stepper v-model="step">
            <v-stepper-header>
                <v-stepper-step step="1" :complete="step > 1">Upload File</v-stepper-step>
                <v-divider></v-divider>
                <v-stepper-step step="2" :complete="step > 2">Review Warnings & Errors</v-stepper-step>
                <v-divider></v-divider>
                <v-stepper-step step="3">Finalize</v-stepper-step>
            </v-stepper-header>

            <v-stepper-items>
                <v-stepper-content step="1">
                    <v-card flat class="overflow-y-auto" style="height: 300px">
                        <v-card-text>
                            Upload a CSV file matching the specifications outlined in the following
                            <a target="_blank"
                               rel="noopener noreferrer"
                               href="https://drive.google.com/file/d/1UB8J9E_cKlezRtgtk3g10ikO9Iqz5lGN/view?usp=sharing">
                                document
                            </a>.
                        </v-card-text>
                        <v-file-input accept=".csv"
                                      label="Click here to add a .csv file"
                                      outlined
                                      @change="fileChanged"></v-file-input>
                    </v-card>
                    <v-btn color="primary"
                           small
                           @click="stepTwo">Continue</v-btn>
                </v-stepper-content>

                <v-stepper-content step="2">
                    <v-card flat class="overflow-y-auto" style="height: 300px">
                        <v-card-title>Warnings</v-card-title>
                        <v-card-text v-if="warnings.length">
                            <p v-for="warning in warnings" v-bind:key="warning">{{warning}}</p>
                        </v-card-text>
                        <v-card-text v-else>
                            No Warnings
                        </v-card-text>

                        <v-card-title>Errors</v-card-title>
                        <v-card-text v-if="errors.length">
                            <p v-for="error in errors" v-bind:key="error">{{error}}</p>
                        </v-card-text>
                        <v-card-text v-else>
                            No Errors
                        </v-card-text>
                    </v-card>
                    <v-btn class="mr-4" @click.native="stepOne" small>Previous</v-btn>
                    <v-btn color="primary" @click="stepThree" small>Continue</v-btn>
                </v-stepper-content>

                <v-stepper-content step="3">
                    <v-card flat class="overflow-y-auto" style="height: 300px">
                        <div>
                            <v-card-title>Import Complete</v-card-title>
                            <v-card-text>
                                # Added: {{ result.added }}
                                # Updated: {{ result.updated }}
                            </v-card-text>
                        </div>
                    </v-card>
                    <v-btn class="mr-4" @click="stepTwo" small>Previous</v-btn>
                    <v-btn color="primary" @click.prevent="close" small>Finish</v-btn>
                </v-stepper-content>
            </v-stepper-items>
        </v-stepper>
    </div>
</template>

<script>
    import axios from 'axios';
export default {
    name: 'import-wizard',
    inject: ['modelRepository', 'assetRepository'],
    props: {
        type: String
    },
    data () {
        return {
            loading: false,
            step: 1,
            file: null,
            importId: null,
            warnings: [],
            errors: [],
            result: {
                added: 0,
                updated: 0
            },
        };
    },
    methods: {
        fileChanged(file) {
            this.file = file;
        },
        stepOne() {
            this.file = null;
            this.warnings = [];
            this.errors = [];
            this.step = 1;
        },
        stepTwo() {
            if (this.file) {
                let formData = new FormData();
                formData.append('file', this.file);
                axios.post(`/import/${this.type}`, formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                }).then(response => {
                    this.importId = response.data.id;
                    this.warnings = response.data.warnings;
                    this.errors = response.data.errors;
                    this.step = 2;
                }).catch(error => {
                    this.errors = [error];
                });
            }
        },
        stepThree() {
            if (this.importId) {
                axios.get(`/import/${this.type}/${this.importId}`)
                    .then(response => {
                        this.result = response.data;
                        this.step = 3;
                    }).catch(error => {
                        this.$emit('import-error', error);
                    })
            }
        },
        close() {
            this.$emit('close-file-chooser');
            this.step = 1;
        }
    }
}   
</script>