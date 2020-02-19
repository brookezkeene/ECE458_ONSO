<template>
    <v-card flat>
        <v-card-title>
            Datacenters & Racks
        </v-card-title>

        <v-container>
            <datacenter-table />
        </v-container>

        <v-container align="center" justify="center">
            <rack-form />
        </v-container>

        <v-container align="center" justify="center">
            <rack-table />
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
    }
}
</script>