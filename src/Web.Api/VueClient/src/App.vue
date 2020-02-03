<template>

    <v-app>
        <v-navigation-drawer v-model="sidebar" app>
            <v-list>
                <v-list-tile v-for="item in menuItems"
                             :key="item.title"
                             :to="item.path">
                    <v-list-tile-action>
                        <v-icon>{{ item.icon }}</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-content>{{ item.title }}</v-list-tile-content>
                </v-list-tile>
            </v-list>
        </v-navigation-drawer>

        <v-toolbar app>
            <span class="hidden-sm-and-up">
                <v-toolbar-side-icon @click="sidebar = !sidebar">
                </v-toolbar-side-icon>
            </span>
            <v-toolbar-title>
                <router-link to="/" tag="span" style="cursor: pointer">
                    {{ appTitle }}
                </router-link>
            </v-toolbar-title>
            <v-spacer></v-spacer>
            <v-toolbar-items class="hidden-xs-only">
                <v-btn flat
                       v-for="item in menuItems"
                       :key="item.title"
                       :to="item.path">
                    <v-icon left dark>{{ item.icon }}</v-icon>
                    {{ item.title }}
                </v-btn>
            </v-toolbar-items>
        </v-toolbar>

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


    export default {

        name: 'App',

        data() {
            return {
                appTitle: 'Hyposoft Tool',
                sidebar: false,
                menuItems: [
                    { title: 'Models', path: '/models', icon: 'mdi-table' },
                    { title: 'Instances', path: '/instances', icon: 'mdi-table' },
                    { title: 'Racks', path: '/racks', icon: 'mdi-table' },
                    { title: 'Users', path: '/users', icon: 'mdi-face-profile-woman' },
                    { title: 'Import/Export', path: '/importexport', icon: 'mdi-import' }
                ]
            }
        },


        components: {
            //Users,
            //Instances,
            //Models,
            //ImportFile,
            //Racks,
        },

    }</script>