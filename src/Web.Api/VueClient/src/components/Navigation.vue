<template>
    <div v-if="!hide">
        <v-navigation-drawer v-model="drawer"
                             class="noPrint"
                             :mini-variant.sync="mini"
                             clipped
                             permanent
                             expand-on-hover
                             app>

            <v-list>
                <div v-for="item in menuItems"
                    :key="item.title">
                    <v-list-item v-if="item.title!='Assets'"
                                 color="primary"
                                 :to="item.path">
                        <v-list-item-icon>
                            <v-icon>{{ item.icon }}</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content>
                            <v-list-item-title>
                                {{ item.title }}
                            </v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>

                    <v-list-group v-else
                                  :prepend-icon="item.icon"
                                  no-action>
                            <template v-slot:activator>
                                <v-list-item-content>
                                    <v-list-item-title>{{ item.title }}</v-list-item-title>
                                </v-list-item-content>
                            </template>

                            <v-list-item v-for="sublink in item.sublinks"
                                         :key="sublink.title"
                                         :to="sublink.path"
                                         color="primary">

                                <v-list-item-content>
                                    <v-list-item-title color="green">
                                        {{ sublink.title }}
                                    </v-list-item-title>
                                </v-list-item-content>
                            </v-list-item>
                    </v-list-group>
                </div>
            </v-list>

            <template v-if="!mini" v-slot:append>
                <div class="pa-2">
                    <v-btn color="primary" block @click="logout">Logout</v-btn>
                </div>
            </template>
        </v-navigation-drawer>

    </div>
</template>

<style>
    @media print {
        .noPrint {
            display: none;
        }
    }
</style>

<script>

    import auth from "../auth";

    export default {

        name: 'navigation',
        data() {
            return {
                sidebar: false,
                drawer: true,
                mini: true,
                menuItems: [
                    { title: 'Models', path: '/models', icon: 'mdi-table-large' },
                    { title: 'Assets', path: '/assets', icon: 'mdi-server', sublinks: [
                        { title: 'Active Assets', path: '/assets'},
                        { title: 'Decommissioned Assets', path: '/decommissioned-assets'},
                    ]},
                    { title: 'Datacenters & Racks', path: '/racks', icon: 'mdi-view-day' },
                    { title: 'Users', path: '/users', icon: 'mdi-account' },
                    { title: 'Reports', path: '/reports', icon: 'mdi-chart-pie' },
                    { title: 'Import/Export', path: '/importexport', icon: 'mdi-file-upload' },
                    { title: 'System Log', path: '/log', icon: 'mdi-post' },
                ],
            }
        },
        computed: {
            hide () {
                return this.$route.path === '/login'; 
            }
        },
        methods: {
            logout() {
                auth.logout();
                this.$router.push({ name: 'login' });
            },
        }
    }
</script>
