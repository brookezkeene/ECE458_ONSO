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
                                <v-btn color="primary" class="mb-2" @click="openExportModels">Export</v-btn>
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
                                <v-btn color="primary" class="mb-2" @click="openExportInstances">Export</v-btn>
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

        <v-dialog v-model="exportModelWizard" max-width="500px">
            <v-card>
                <export-wizard v-on:close-file-chooser="closeExport" v-bind:forModel="forModel"></export-wizard>
            </v-card>
        </v-dialog>

        <v-dialog v-model="exportInstanceWizard" max-width="500px">
            <v-card>
                <export-wizard v-on:close-file-chooser="closeExport" v-bind:forModel="forModel"></export-wizard>
            </v-card>
        </v-dialog>
    </v-card>
</template>

<script>

import ModelImportFormatInfo from "./ModelImportFormatInfo"
import InstanceImportFormatInfo from "./InstanceImportFormatInfo"
import ImportWizard from "./ImportWizard"
import ExportWizard from "./ExportWizard"
import Auth from "../auth"

export default {
    data () {
        return {
            loading: false,       
            extrainfomodel: false,
            extrainfoinstance: false,
            importModelWizard: false,
            importInstanceWizard: false,
            exportModelWizard: false,
            exportInstanceWizard: false,
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
        exportModelWizard(val) {
            val || this.closeExport("model")
        },
        exportInstanceWizard(val) {
            val || this.closeExport("instance")
        }
    },
    components: {
      ModelImportFormatInfo,
      InstanceImportFormatInfo,
      ImportWizard,
      ExportWizard
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
        openExportModels() {
            this.exportModelWizard = true
            this.forModel = true
        },
        openExportInstances() {
            this.exportInstanceWizard = true
            this.forModel = false
        },
        closeExport(type) {
            if (type === "model") {
                this.exportModelWizard = false
            }
            else {
                this.exportInstanceWizard = false
            }
        }
    }
    
}
</script>