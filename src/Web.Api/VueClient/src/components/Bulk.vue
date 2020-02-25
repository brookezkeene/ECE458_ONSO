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
                        Assets
                        <v-spacer></v-spacer>
                        <v-icon v-if="admin" @click="showAssetInfo">mdi-information</v-icon>
                    </v-card-title>
                    <v-card-actions class="justify-center">
                        <v-container>
                            <v-row align="center" justify="center">
                                <v-btn v-if="admin" color="primary" class="mb-2" @click="openImportAssets">Import</v-btn>
                            </v-row>
                            <v-row align="center" justify="center">
                                <v-btn color="primary" class="mb-2" @click="startExportAssets">Export</v-btn>
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

        <v-dialog v-model="extrainfoasset">
            <v-card>
                <asset-import-format-info></asset-import-format-info>
            </v-card>
        </v-dialog>

        <v-dialog v-model="importModelWizard" max-width="500px">
            <v-card>
                <import-wizard v-on:close-file-chooser="closeImport" v-bind:forModel="forModel"></import-wizard>
            </v-card>
        </v-dialog>

        <v-dialog v-model="importAssetWizard" max-width="500px">
            <v-card>
                <import-wizard v-on:close-file-chooser="closeImport" v-bind:forModel="forModel"></import-wizard>
            </v-card>
        </v-dialog>

        <v-dialog v-model="exportModelDialog" max-width="300px">
            <export-model-wizard v-on:close-model-export="closeExport('model')"></export-model-wizard>
        </v-dialog>

        <v-dialog v-model="exportAssetDialog" max-width="300px">
            <export-asset-wizard v-on:close-asset-export="closeExport('asset')"></export-asset-wizard>
        </v-dialog>
    </v-card>
</template>

<script>

import ModelImportFormatInfo from "./ModelImportFormatInfo"
import AssetImportFormatInfo from "./AssetImportFormatInfo"
import ImportWizard from "./ImportWizard"
import ExportModelWizard from "./ExportModelWizard"
import ExportAssetWizard from "./ExportAssetWizard"
import Auth from "../auth"

export default {
    data () {
        return {
            loading: false,       
            extrainfomodel: false,
            extrainfoasset: false,
            importModelWizard: false,
            importAssetWizard: false,
            exportModelDialog: false,
            exportAssetDialog: false,
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
        importAssetWizard(val) {
            val || this.closeImport("asset")
        },
        exportDialog(val) {
            val || this.closeExport()
        }
    },
    components: {
      ModelImportFormatInfo,
      AssetImportFormatInfo,
      ImportWizard,
      ExportModelWizard,
      ExportAssetWizard
    },
    methods: {
        showModelInfo() {
            this.extrainfomodel = true
        },
        showAssetInfo() {
            this.extrainfoasset = true
        },
        openImportModels() {
            this.importModelWizard = true
            this.forModel = true
        },
        openImportAssets() {
            this.importAssetWizard = true
            this.forModel = false

        },
        closeImport(type) {
            if (type === "model") {
                this.importModelWizard = false
            }
            else {
                this.importAssetWizard = false
            }
        },
        closeExport(type) {
            if (type === "model") {
                this.exportModelDialog = false
            }
            else {
                this.exportAssetDialog = false
            }
        },
        startExportModels() {
            this.exportModelDialog = true
        },
        startExportAssets() {
            this.exportAssetDialog = true
        },
    }
}
</script>