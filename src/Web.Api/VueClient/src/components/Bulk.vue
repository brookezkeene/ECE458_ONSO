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
                                <v-btn v-if="admin" color="primary" class="mb-2" @click="openFileChooser">Import</v-btn>
                            </v-row>
                            <v-row align="center" justify="center">
                                <v-btn color="primary" class="mb-2">Export</v-btn>
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
                        <v-icon v-if="admin" @click="showModelInfo">mdi-information</v-icon>
                    </v-card-title>
                    <v-card-actions class="justify-center">
                        <v-container>
                            <v-row align="center" justify="center">
                                <v-btn v-if="admin" color="primary" class="mb-2" @click="openFileChooser">Import</v-btn>
                            </v-row>
                            <v-row align="center" justify="center">
                                <v-btn color="primary" class="mb-2">Export</v-btn>
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
        <v-dialog v-model="importWizard" max-width="500px">
            <v-card>
              <import-wizard v-on:close-file-chooser="closeFileChooser"></import-wizard>
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
            importWizard: false,
        };
    },
    computed: {
        admin() {
            return Auth.isAdmin()
        },
    },
    watch: {
        importWizard(val) {
            val || this.closeFileChooser()
        },
    },
    components: {
      ModelImportFormatInfo,
      InstanceImportFormatInfo,
      ImportWizard,
    },
    methods: {
        showModelInfo() {
            this.extrainfomodel = true
        },
        showInstanceInfo() {
            this.extrainfoinstance = true
        },
        openFileChooser() {
            this.importWizard = true
        },
        closeFileChooser() {
            this.importWizard = false
        }
    }
    
}
</script>