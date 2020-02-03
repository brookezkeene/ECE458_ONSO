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
                    <v-card flat>
                        <v-card-text>
                            Upload a CSV file matching the specifications outlined in the following
                            <a target="_blank"
                               rel="noopener noreferrer"
                               href="https://piazza.com/redirect/s3?bucket=uploads&prefix=attach%2Fk4u27qnccr45oo%2Fis4xdnkb8px4ee%2Fk5zjop579k59%2FECE458__Bulk_Format_Proposal3.pdf">
                            document</a>.
                        </v-card-text>
                        <v-file-input accept=".csv"
                                      label="Click here to add a .csv file"
                                      outlined
                                      @change="fileUpload"
                                      ></v-file-input>
                    </v-card>
                    <v-btn color="primary"
                           small="true"
                           @click="step = 2">Continue</v-btn>
                </v-stepper-content>

                <v-stepper-content step="2">
                    <v-card flat>
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
                    <v-btn class="mr-4" @click.native="step = 1" small="true">Previous</v-btn>
                    <v-btn color="primary" @click="step = 3" small="true">Continue</v-btn>
                </v-stepper-content>

                <v-stepper-content step="3">
                    <v-card flat>
                        
                    </v-card>
                    <v-btn class="mr-4" @click="step = 2" small="true">Previous</v-btn>
                    <v-btn color="primary" @click.prevent="close" small="true">Save</v-btn>
                </v-stepper-content>
            </v-stepper-items>
        </v-stepper>
    </div>
</template>

<script>
export default {
    name: 'file-chooser-card',
    props: ['filechoosercard'],
    data () {
        return {
            loading: false,
            step: 1,
            fileChosen: false,
            warnings: ["warning1", "warning2"],   // array to store all warnings, empty if none
            errors: [],     // array to store all errors, empty if none
        };
    },
    methods: {
        fileUpload(e) { // not working
            var files = e.target.files || e.dataTransfer.files;
            if (files.length > 0) {
                this.fileChosen = true;
                // API call?
            }
            
        },
        // API calls to get warnings and errors
        close() {
            alert('Data Successfully Imported.');
            this.$emit('close-file-chooser');
            this.step = 1;
        }
    }
}   
</script>