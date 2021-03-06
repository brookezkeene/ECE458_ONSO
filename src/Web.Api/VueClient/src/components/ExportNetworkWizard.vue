﻿<template>
    <div v-if="!loading">
        <v-stepper v-model="step">
            <v-stepper-header>
                <v-stepper-step step="1" :complete="step > 1">Select Filters</v-stepper-step>
                <v-divider></v-divider>
                <v-stepper-step step="2" :complete="close">Confirm</v-stepper-step>
            </v-stepper-header>

            <v-stepper-items>
                <v-stepper-content step="1">
                    <v-card flat class="overflow-y-auto" style="height: 300px">

                        <v-card-title>Export</v-card-title>
                        <v-card flat>
                            <v-container fill-height fluid>
                                <v-label>
                                    Input an asset host name to filter result by
                                </v-label>
                                <v-text-field v-model="query.HostName">
                                </v-text-field>
                                <v-label>
                                    Input a model vendor name to filter result by
                                </v-label>
                                <v-text-field v-model="query.Search">
                                </v-text-field>
                                <v-label>
                                    Input a start rack to filter result by
                                </v-label>
                                <v-text-field v-model="startRack"
                                              :rules="[rules.rackRules]">
                                </v-text-field>
                                <v-label>
                                    Input an end rack to filter result by
                                </v-label>
                                <v-text-field v-model="endRack"
                                              :rules="[rules.rackRules]">
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
                               href="https://d1b10bmlvqabco.cloudfront.net/attach/k4u27qnccr45oo/is4xdnkb8px4ee/k90vqcj1j0tn/ECE458__Bulk_Format_Proposal6.pdf">
                                document
                            </a>.
                            <div class="pt-10">
                                <downloadCsv :data="exportRaw"
                                             name="networkconnections.csv"
                                             type="csv">
                                    <v-spacer></v-spacer>
                                    <v-btn>
                                        <b>Download</b>
                                    </v-btn>
                                </downloadCsv>
                            </div>
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
                startRack: '',
                endRack: '',
                rules: {
                    rackRules: v => /[a-zA-Z][0-9]+$/.test(v) || 'Network port name cannot contain whitespace'
                },
                networkErrorDialog: false
            };
        },
        methods: {
            async setStep2() {
                if (/^(?:|[a-zA-Z][0-9]+)*$/.test(this.startRack) && /^(?:|[a-zA-Z][0-9]+)*$/.test(this.endRack)) {
                    if (this.startRack.length != 0) {
                        this.query.StartRow = this.startRack[0];
                        this.query.StartCol = parseInt(this.startRack.substring(1));
                    }
                    if (this.endRack.length != 0) {
                        this.query.EndRow = this.endRack[0];
                        this.query.EndCol = parseInt(this.endRack.substring(1));
                    }
                }
                this.exportRaw = await this.exportRepository.exportNetwork(this.query);

                /* eslint-disable no-unused-vars, no-console */
                console.log(this.exportRaw.length);
                console.log(this.exportRaw);
                console.log("This is inside of export raw");

                this.step = 2
            },
            close() {
                this.step = 1;
                this.$emit('close-network-export');
            },
            handleError() {
                this.networkErrorDialog = true;
            },
        }
    }
</script>