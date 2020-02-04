<template>
    <v-card flat>
        <v-card-title>
            Bulk Import & Export
        </v-card-title>
        <v-container fill-height fluid>
            <v-col>
                <v-card>
                    <v-card-title class="justify-center">
                        <v-icon v-if="admin" color="white">mdi-information</v-icon>
                        <v-spacer></v-spacer>
                        Models
                        <v-spacer></v-spacer>
                        <v-icon v-if="admin" @click="showModelInfo">mdi-information</v-icon>
                    </v-card-title>
                    <v-card-actions class="justify-center">
                        <v-container>
                            <v-row align="center" justify="center">
                                <v-btn v-if="admin" color="primary" class="mb-2" @click="openImportModels">Import</v-btn>
                            </v-row>
                            <v-row align="center" justify="center">
                                <v-btn color="primary" class="mb-2" @click="startExportModels">Export</v-btn>
                            </v-row>
                        </v-container>
                    </v-card-actions>
                </v-card>
            </v-col>
            <v-col>
                <v-card>
                    <v-card-title class="justify-center">
                        <v-icon v-if="admin" color="white">mdi-information</v-icon>
                        <v-spacer></v-spacer>
                        Instances
                        <v-spacer></v-spacer>
                        <v-icon v-if="admin" @click="showInstanceInfo">mdi-information</v-icon>
                    </v-card-title>
                    <v-card-actions class="justify-center">
                        <v-container>
                            <v-row align="center" justify="center">
                                <v-btn v-if="admin" color="primary" class="mb-2" @click="openImportInstances">Import</v-btn>
                            </v-row>
                            <v-row align="center" justify="center">
                                <v-btn color="primary" class="mb-2" @click="startExportInstances">Export</v-btn>
                            </v-row>
                        </v-container>
                    </v-card-actions>
                </v-card>
            </v-col>
        </v-container>

        <v-dialog v-model="extrainfomodel">
            <v-card>
                <model-import-format-info></model-import-format-info>
            </v-card>
        </v-dialog>

        <v-dialog v-model="extrainfoinstance">
            <v-card>
                <instance-import-format-info></instance-import-format-info>
            </v-card>
        </v-dialog>

        <v-dialog v-model="importModelWizard" max-width="500px">
            <v-card>
                <import-wizard v-on:close-file-chooser="closeImport" v-bind:forModel="forModel"></import-wizard>
            </v-card>
        </v-dialog>

        <v-dialog v-model="importInstanceWizard" max-width="500px">
            <v-card>
                <import-wizard v-on:close-file-chooser="closeImport" v-bind:forModel="forModel"></import-wizard>
            </v-card>
        </v-dialog>

        <v-dialog v-model="exportDialog" max-width="300px">
            <v-card>
                <v-container fill-height fluid>
                    <v-card-title class="justify-center">
                        Exporting .csv file... 
                    </v-card-title>
                    <v-progress-circular indeterminate
                                         color="primary"></v-progress-circular>
                </v-container>
            </v-card>
        </v-dialog>
    </v-card>
</template>

<script>

import ModelImportFormatInfo from "./ModelImportFormatInfo"
import InstanceImportFormatInfo from "./InstanceImportFormatInfo"
import ImportWizard from "./ImportWizard"
import Auth from "../auth"

export default {
    data () {
        return {
            loading: false,       
            extrainfomodel: false,
            extrainfoinstance: false,
            importModelWizard: false,
            importInstanceWizard: false,
            exportDialog: false,
            forModel: false,
        };
    },
    computed: {
        admin() {
            return Auth.isAdmin()
        },
    },
    watch: {
        importModelWizard(val) {
            val || this.closeImport("model")
        },
        importInstanceWizard(val) {
            val || this.closeImport("instance")
        },
        exportDialog(val) {
            val || this.closeExport()
        }
    },
    components: {
      ModelImportFormatInfo,
      InstanceImportFormatInfo,
      ImportWizard
    },
    methods: {
        showModelInfo() {
            this.extrainfomodel = true
        },
        showInstanceInfo() {
            this.extrainfoinstance = true
        },
        openImportModels() {
            this.importModelWizard = true
            this.forModel = true
        },
        openImportInstances() {
            this.importInstanceWizard = true
            this.forModel = false

        },
        closeImport(type) {
            if (type === "model") {
                this.importModelWizard = false
            }
            else {
                this.importInstanceWizard = false
            }
        },
        startExportModels() {
            this.exportDialog = true
        },
        startExportInstances() {
            this.exportDialog = true
        },
        closeExport() {
            this.exportDialog = false
        }
    }
}
</script>