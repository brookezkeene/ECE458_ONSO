<template>
    <div v-if="!hide">
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
                <span class="font-weight-light">
                    {{username}}
                </span>
            </div>


        </v-app-bar>

    </div>

</template>

<script>


    export default {

        name: 'app-bar',
        created() {
            this.$store.dispatch('loadUsername');
            this.$store.dispatch('loadPermissions');
        },
        computed: {
            username() {
                /*eslint-disable*/
                console.log(this.$store.state.username);
                return this.$store.state.username;
            },
            hide () {
                return this.$route.path === '/login';
            }
        },
        data() {
            return {
                appTitle: 'Hyposoft Tool',

            }
        }
    }
</script>