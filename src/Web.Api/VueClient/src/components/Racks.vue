<template>
    <v-card flat>
        <v-card-title>
            Datacenters & Racks
        </v-card-title>

        <v-container>
            <datacenter-table />
        </v-container>

        <v-container align="center" justify="center">
            <rack-table :updateData="updateData" v-on:updated="blockUpdate" ></rack-table>
        </v-container>

        <v-container align="center" justify="center" class="justify-center">
            <rack-form v-on:update-table="allowUpdate"></rack-form>
        </v-container>
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
        }),
        methods: { // This might slow things down if we have a lot of racks to get from the backend !!!
            allowUpdate() {
                this.updateData = true;
            },
            blockUpdate() {
                this.updateData = false;
            },
        }
    }
</script>