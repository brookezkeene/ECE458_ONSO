<template>
    <v-app>
        <appbar></appbar>
        <navigation></navigation>
        <v-content>
            <v-container fluid>
                <router-view></router-view>
            </v-container>
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
    import appbar from "@/components/AppBar"
    import navigation from "@/components/Navigation"

    export default {
        name: 'App',

        components: {
            appbar,
            navigation
        }
    }
</script>