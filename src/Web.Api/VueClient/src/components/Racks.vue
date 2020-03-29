<template>
    <v-card flat>
        <v-card-title>
            Datacenters
        </v-card-title>

        <v-container>
            <datacenter-table />
        </v-container>

        <v-card-title class="pt-10">
            Racks
        </v-card-title>

        <v-container align="center" justify="center" class="justify-center">
            <rack-form v-on:update-create="allowUpdate('create')"
                       v-on:update-delete="allowUpdate('delete')"
                       v-on:error-create="showError('create')"
                       v-on:error-delete="showError('delete')"
                       v-on:error-datacenter="showError('datacenter')"></rack-form>
        </v-container>

        <v-container align="center" justify="center">
            <rack-table :updateData="updateData" v-on:updated="blockUpdate"></rack-table>
        </v-container>

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

<style>
    .v-snackbar {
        border-color: green;
    }
</style>

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
                color: ''
            }
        }),
        methods: { // This might slow things down if we have a lot of racks to get from the backend !!!
            allowUpdate(action) {
                this.updateData = true;
                this.updated = true;
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'green lighten-4';
                if (action === 'create') {
                    this.updateSnackbar.message = 'Successfully created racks';
                }
                if (action === 'delete') {
                    this.updateSnackbar.message = 'Successfully deleted racks';
                }
            },
            showError(action) {
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'red lighten-4';
                if (action === 'create') {
                    this.updateSnackbar.message = 'Could not create racks';
                }
                if (action === 'delete') {
                    this.updateSnackbar.message = 'Could not delete racks';
                }
                if (action === 'datacenter') {
                    this.updateSnackbar.message = 'You do not have permission to modify the selected datacenter';
                }
            },
            blockUpdate() {
                this.updateData = false;
            },
        }
    }
</script>