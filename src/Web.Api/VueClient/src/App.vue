<template>
    <v-app>
        <v-content>
            <router-view></router-view>
        </v-content>
    </v-app>
</template>

<style>
    @media print {
        .noPrint {
            display: none;
        }
    } 
</style>

<script>
    export default {
        name: 'App'
    }
</script>