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
                <div v-for="item in filteredMenuItems"
                    :key="item.title">
                    <v-list-item v-if="item.title!='Assets' && item.title!='Sites'"
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
                                         @click="route(sublink)"
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
                        { title: 'Active Assets', name: 'assets'},
                        { title: 'Decommissioned', name: 'decommissioned-assets' },
                        { title: 'Offline', name: '/offline-assets'}
                    ]},
                    { title: 'Change Planner', path: '/change-plan', icon: 'mdi-clipboard-list-outline'},
                    { title: 'Sites', path: '/sites', icon: 'mdi-factory', sublinks: [
                        { title: 'Datacenters', name: 'datacenters', type: 'datacenters'},
                        { title: 'Offline Storage', name: 'offline-storage', type: 'offline-storage'}
                    ]},
                    { title: 'Racks', path: '/racks', icon: 'mdi-view-day' },
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
            },
            filteredMenuItems() {
                var newMenu = this.menuItems;
                if (!this.$store.getters.hasAssetPermission && !this.$store.getters.isAdmin) {
                    newMenu = newMenu.filter(h => h.title !== "Change Planner");
                }
                if (!this.$store.getters.hasAuditPermission && !this.$store.getters.isAdmin) {
                    newMenu = newMenu.filter(h => h.title !== "System Log");
                }
                return newMenu
            },
        },
        methods: {
            logout() {
                auth.logout();
                this.$router.push({ name: 'login' });
            },
            route(item) {
                if (item.type != undefined) {
                    this.$router.push({ name: item.name, params: { type: item.type } });
                } else {
                    this.$router.push({ name: item.name });
                }
            }
        }
    }
</script>
