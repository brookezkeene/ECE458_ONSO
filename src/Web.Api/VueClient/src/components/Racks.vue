<template>
    <v-card>
        <v-card-title>
            Racks
        </v-card-title>

        <v-container align="center" justify="center">
            <rack-form />
        </v-container>

        <v-container align="center" justify="center">
            <rack-table />
        </v-container>
    </v-card>
</template>

<script>
import RackForm from "./RackForm"
import RackTable from "./RackTable"

export default {
    components: {
        RackForm,
        RackTable
    }
}
</script>