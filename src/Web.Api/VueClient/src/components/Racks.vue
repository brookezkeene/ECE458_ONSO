<template>
    <v-card flat>
        <v-card-title>
            Racks
        </v-card-title>

        <v-container align="center" justify="center">
            <usage-report />
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
import UsageReport from "./UsageReport"
import RackForm from "./RackForm"
import RackTable from "./RackTable"

export default {
        components: {
        UsageReport,
        RackForm,
        RackTable
    }
}
</script>