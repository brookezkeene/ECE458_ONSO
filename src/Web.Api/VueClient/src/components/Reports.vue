<template>
    <v-card flat>
        <v-card-title>
            Reports
        </v-card-title>

        <v-container align="center" justify="center">
            <usage-report />
        </v-container>
    </v-card>
</template>

<script>
import UsageReport from "./UsageReport"

export default {
        components: {
        UsageReport,
    }
}</script>