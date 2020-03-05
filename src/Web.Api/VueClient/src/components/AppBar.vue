<template>
    <div>
        <v-app-bar color="primary"
                   dense
                   dark
                   flat>

            <v-toolbar-title>HypoSoft Tool</v-toolbar-title>

            <v-spacer></v-spacer>
            <template>
                <v-icon small>mdi-account</v-icon>
                <text-area color="white"> Hello {{username}}! </text-area>
            </template>
        </v-app-bar>
    </div>
</template>


<script>
    import auth from "../auth";
    export default {
        name: 'app-bar',
        computed: {
            username() {
                return auth.username()
            },
        }
    }
</script>