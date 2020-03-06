<template>
    <div>
        <v-app-bar color="primary"
                   dense
                   dark
                   app
                   clipped-left>

            <v-toolbar-title>
                <v-avatar size="36"
                          left>
                    <v-img src="@/assets/hyposoft_logo.png"></v-img>
                </v-avatar>
                <router-link to="/" tag="span" style="cursor: pointer">
                    {{ appTitle }}
                </router-link>
            </v-toolbar-title>

            <v-spacer></v-spacer>


            <div>
                <v-icon small>mdi-account-circle</v-icon>
                <p class="font-weight-light">
                    Hello {{username}}!
                </p>
            </div>


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
        },
        data() {
            return {
                appTitle: 'Hyposoft Tool',

            }
        }
    }
</script>