<template>
    <v-card flat>
        <v-card-title>
            Datacenters & Racks
        </v-card-title>

        <v-container>
            <datacenter-table />
        </v-container>

        <v-container align="center" justify="center">
            <rack-table :updateData="updateData" v-on:updated="blockUpdate"></rack-table>
        </v-container>

        <v-container align="center" justify="center" class="justify-center">
            <rack-form v-on:update-create="allowUpdateCreate()" 
                       v-on:update-delete="allowUpdateDelete()"></rack-form>
        </v-container>

        <v-snackbar v-model="updateSnackbar.show"
                    :bottom=true
                    :color="updateSnackbar.color"
                    :timeout=5000>
            {{updateSnackbar.message}}
            <v-btn dark
                   text
                   @click="updateSnackbar.show = false">
                Close
            </v-btn>
        </v-snackbar>
    </v-card>
</template>

<script>
import DatacenterTable from "./DatacenterTable"
import RackForm from "./RackForm"
import RackTable from "./RackTable"

    export default {
        components: {
            DatacenterTable,
            RackForm,
            RackTable
        },
        data: () => ({
            updateData: false, // This might slow things down if we have a lot of racks to get from the backend !!!
            updateSnackbar: {
                show: false,
                message: '',
                color: 'green lighten-2'
            }
        }),
        methods: { // This might slow things down if we have a lot of racks to get from the backend !!!
            allowUpdateCreate() {
                this.updateData = true;
                this.updated = true;
                this.updateSnackbar.show = true;
                this.updateSnackbar.message = 'Successfully created racks';
            },
            allowUpdateDelete() {
                this.updateData = true;
                this.updated = true;
                this.updateSnackbar.show = true;
                this.updateSnackbar.message = 'Successfully deleted racks';
            },
            blockUpdate() {
                this.updateData = false;
            },
        }
    }
</script>