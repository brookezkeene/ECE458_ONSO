<template>
    <v-card>
        <v-card-title>
            Bulk Import & Export
        </v-card-title>
        <v-container fill-height fluid>
            <v-row align="center"
                justify="center">
            </v-row>
            <v-col>
                <v-card center>
                    <span class="group pa-2">
                        <v-row>
                            <v-card-title> Models </v-card-title>
                            <v-icon @click="showModelInfo">mdi-information</v-icon>
                        </v-row>
                    </span>
                    <v-col>
                        <v-btn color = "primary" class="mb-2" @click="openFileChooser">Import</v-btn>
                    </v-col>
                    <v-col>
                        <v-btn color = "primary" class="mb-2">Export</v-btn>
                    </v-col>
                </v-card>
            </v-col> 
            <v-col>
                <v-card center>
                    <span class="group pa-2">
                        <v-row>
                            <v-card-title> Instances </v-card-title>
                            <v-icon @click="showInstanceInfo">mdi-information</v-icon>
                        </v-row>
                    </span>
                    <v-col>
                        <v-btn color = "primary" class="mb-2" @click="openFileChooser">Import</v-btn>
                    </v-col>
                    <v-col>
                        <v-btn color = "primary" class="mb-2">Export</v-btn>
                    </v-col>
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
        <v-dialog v-model="filechoosercard" max-width="500px">
            <v-card>
              <file-chooser-card></file-chooser-card>
            </v-card>
        </v-dialog>
    </v-card>
</template>

<script>

import ModelImportFormatInfo from "./ModelImportFormatInfo"
import InstanceImportFormatInfo from "./InstanceImportFormatInfo"
import FileChooserCard from "./FileChooserCard"

export default {
    data () {
        return {
            loading: false,       
            extrainfomodel: false,
            extrainfoinstance: false,
            filechoosercard: false,
        };
    },
    components: {
      ModelImportFormatInfo,
      InstanceImportFormatInfo,
      FileChooserCard,
    },
    methods: {
        showModelInfo() {
            this.extrainfomodel = true
        },
        showInstanceInfo() {
            this.extrainfoinstance = true
        },
        openFileChooser(){
            this.filechoosercard = true
        }
    }
    
}
</script>