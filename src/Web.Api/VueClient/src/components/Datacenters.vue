<template>
    <v-card flat>
        <v-card-title>
            {{}}
        </v-card-title>

        <v-container>
            <datacenter-table />
        </v-container>

    </v-card>
</template>

<script>
import DatacenterTable from "./DatacenterTable"

    export default {
        components: {
            DatacenterTable,
        },
        data: () => ({

        }),
        methods: { 
        }
    }
</script>