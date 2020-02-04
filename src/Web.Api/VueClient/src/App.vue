<template>
    <v-app>
        <v-navigation-drawer v-model="drawer"
                             v-if="loggedIn"
                             :mini-variant.sync="mini"
                             permanent
                             app>

            <v-list-item class="px-2">
                <v-list-item-avatar>
                    <v-img src="./components/example/hyposoft_logo.png"></v-img>
                </v-list-item-avatar>

                <v-list-item-title>
                    <router-link to="/" tag="span" style="cursor: pointer">
                        {{ appTitle }}
                    </router-link>
                </v-list-item-title>

                <v-btn icon
                       color="primary"
                       @click.stop="mini = !mini">
                    <v-icon>mdi-chevron-left</v-icon>
                </v-btn>
            </v-list-item>

            <v-list>
                <v-list-item v-for="item in menuItems"
                             :key="item.title"
                             :to="item.path"
                             color="primary">
                    <v-list-item-icon>
                        <v-icon>{{ item.icon }}</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                        <v-list-item-title>
                            {{ item.title }}
                        </v-list-item-title>
                    </v-list-item-content>
                </v-list-item>
            </v-list>

            <template v-if="!mini" v-slot:append>
                <div class="pa-2">
                    <v-btn color="primary" block>Logout</v-btn>
                </div>
            </template>
        </v-navigation-drawer>

        <v-content>
            <router-view></router-view>
        </v-content>

    </v-app>

</template>

<script>
    //import Users from '@/components/Users';
    //import Instances from '@/components/Instances';
    //import Models from '@/components/Models';
    //import ImportFile from '@/components/ImportFile';
    //import Racks from '@/components/Racks';
    import Auth from "./auth"


    export default {

        name: 'App',

        data() {
            return {
                appTitle: 'Hyposoft Tool',
                sidebar: false,
                drawer: true,
                mini: true,
                menuItems: [
                    { title: 'Models', path: '/models', icon: 'mdi-table-large' },
                    { title: 'Instances', path: '/instances', icon: 'mdi-server' },
                    { title: 'Racks', path: '/racks', icon: 'mdi-view-day' },
                    { title: 'Users', path: '/users', icon: 'mdi-account' },
                    { title: 'Import/Export', path: '/importexport', icon: 'mdi-file-upload' }
                ]
            }
        },

        computed: {
            loggedIn() {
                return Auth.loggedIn();
            }
        },

        components: {
            //Users,
            //Instances,
            //Models,
            //ImportFile,
            //Racks,
        },

    }
</script>