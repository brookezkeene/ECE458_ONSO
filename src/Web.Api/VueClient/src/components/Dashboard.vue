<template>
    <v-app>
        <v-navigation-drawer v-model="drawer"
                             class="noPrint"
                             :mini-variant.sync="mini"
                             permanent
                             expand-on-hover
                             app>

            <v-list-item class="px-2">
                <v-list-item-avatar>
                    <v-img src="@/assets/hyposoft_logo.png"></v-img>
                </v-list-item-avatar>

                <v-list-item-title>
                    <router-link to="/" tag="span" style="cursor: pointer">
                        {{ appTitle }}
                    </router-link>
                </v-list-item-title>
            </v-list-item>

            <v-list>
                <v-list-item v-for="item in activeTabs"
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
                    <v-btn color="primary" block @click="logout">Logout</v-btn>
                </div>
            </template>
        </v-navigation-drawer>

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

    import auth from "../auth"
    export default {

        name: 'Dashboard',
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
                    { title: 'Reports', path: '/reports', icon: 'mdi-chart-pie' },
                    { title: 'Import/Export', path: '/importexport', icon: 'mdi-file-upload' }
                ]
            }
        },
        computed: {
            activeTabs : function() {
                return this.menuItems.filter(m => auth.isAdmin() || !['Users', 'Import/Export'].includes(m.title));
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
