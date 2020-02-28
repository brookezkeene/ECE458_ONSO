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
            <v-col>
                <v-card>
                    <v-card-title class="justify-center">
                        <v-icon v-if="admin" color="white">mdi-information</v-icon>
                        <v-spacer></v-spacer>
                        Networks
                        <v-spacer></v-spacer>
                        <v-icon v-if="admin" @click="showNetworkInfo">mdi-information</v-icon>
                    </v-card-title>
                    <v-card-actions class="justify-center">
                        <v-container>
                            <v-row align="center" justify="center">
                                <v-btn v-if="admin" color="primary" class="mb-2" @click="openImportNetworks">Import</v-btn>
                            </v-row>
                            <v-row align="center" justify="center">
                                <v-btn color="primary" class="mb-2" @click="startExportNetworks">Export</v-btn>
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

        <v-dialog v-model="extrainfonetwork">
            <v-card>
                <network-import-format-info></network-import-format-info>
            </v-card>
        </v-dialog>

        <v-dialog v-model="importModelWizard" max-width="500px">
            <v-card>
                <import-wizard type="models" v-on:close-file-chooser="closeImport" @import-error="throwError(e)"></import-wizard>
            </v-card>
        </v-dialog>

        <v-dialog v-model="importAssetWizard" max-width="500px">
            <v-card>
                <import-wizard v-on:close-file-chooser="closeImport"></import-wizard>
            </v-card>
        </v-dialog>

        <v-dialog v-model="importNetworkWizard" max-width="500px">
            <v-card>
                <import-wizard v-on:close-file-chooser="closeImport"></import-wizard>
            </v-card>
        </v-dialog>

        <v-dialog v-model="exportModelDialog" max-width="500px">
            <export-model-wizard v-on:close-model-export="closeExport('model')"></export-model-wizard>
        </v-dialog>

        <v-dialog v-model="exportAssetDialog" max-width="500px">
            <export-asset-wizard v-on:close-asset-export="closeExport('asset')"></export-asset-wizard>
        </v-dialog>

        <v-dialog v-model="exportNetworkDialog" max-width="500px">
            <export-network-wizard v-on:close-network-export="closeExport('network')"></export-network-wizard>
        </v-dialog>

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
</template>

<script>

import ModelImportFormatInfo from "./ModelImportFormatInfo"
import AssetImportFormatInfo from "./AssetImportFormatInfo"
import NetworkImportFormatInfo from "./NetworkImportFormatInfo"
import ImportWizard from "./ImportWizard"
import ExportModelWizard from "./ExportModelWizard"
import ExportAssetWizard from "./ExportAssetWizard"
import ExportNetworkWizard from "./ExportNetworkWizard"
import Auth from "../auth"

export default {
    data () {
        return {
            loading: false,       
            extrainfomodel: false,
            extrainfoasset: false,
            extrainfonetwork: false,
            importModelWizard: false,
            importAssetWizard: false,
            importNetworkWizard: false,
            exportModelDialog: false,
            exportAssetDialog: false,
            exportNetworkDialog: false,
            
            updateSnackbar: {
                show: false,
                message: '',
                color: 'red lighten-2'
            }
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
        importNetworkWizard(val) {
            val || this.closeImport("network")
        },
        exportDialog(val) {
            val || this.closeExport()
        }
    },
    components: {
      ModelImportFormatInfo,
      AssetImportFormatInfo,
      NetworkImportFormatInfo,
      ImportWizard,
      ExportModelWizard,
      ExportAssetWizard,
      ExportNetworkWizard
    },
    methods: {
        showModelInfo() {
            this.extrainfomodel = true
        },
        showAssetInfo() {
            this.extrainfoasset = true
        },
        showNetworkInfo() {
            this.extrainfonetwork = true
        },
        openImportModels() {
            this.importModelWizard = true
        },
        openImportAssets() {
            this.importAssetWizard = true
        },
        openImportNetworks() {
            this.importNetworkWizard = true
        },
        closeImport(type) {
            if (type === "model") {
                this.importModelWizard = false
            }
            else if (type === "asset") {
                this.importAssetWizard = false
            }
            else {
                this.importNetworkWizard = false
            }
        },
        closeExport(type) {
            if (type === "model") {
                this.exportModelDialog = false
            }
            else if (type === "asset") {
                this.exportAssetDialog = false
            }
            else {
                this.exportNetworkDialog = false
            }
        },
        startExportModels() {
            this.exportModelDialog = true
        },
        startExportAssets() {
            this.exportAssetDialog = true
        },
        startExportNetworks() {
            this.exportNetworkDialog = true
        },
        throwError(e) {
            // this.updateSnackbar.message = e; ERROR IS UNDEFINED
            this.updateSnackbar.message = e + 'Error importing models';
            this.updateSnackbar.color = "red lighten-2";
            this.updateSnackbar.show = true;
        }
    }
}
</script>