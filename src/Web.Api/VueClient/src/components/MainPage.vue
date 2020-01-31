<template>
        <v-app>
            <v-app-bar>
                <v-toolbar-title>
                    Hyposoft Tool
                </v-toolbar-title>
                </v-app-bar>
                <v-content>
                    <v-container fluid>
                        <v-tabs>
                            <v-tab ripple>
                                Models
                            </v-tab>
                            <v-tab ripple>
                                Instances
                            </v-tab>
                            <v-tab ripple>
                                Racks
                            </v-tab>
                            <v-tab ripple>
                                Users
                            </v-tab>
                            <v-tab ripple>
                                Import/Export
                            </v-tab>
                            <v-tab-item>
                                <Models />
                            </v-tab-item>
                            <v-tab-item>
                                <Instances />
                            </v-tab-item>
                            <v-tab-item>
                                <Racks />
                            </v-tab-item>
                            <v-tab-item>
                                <Users />
                            </v-tab-item>
                            <v-tab-item>
                                <ImportFile />
                            </v-tab-item>
                        </v-tabs>
                    </v-container>
                </v-content>
            </v-app>
</template>

<script>

import Users from '@/components/Users';
import Instances from '@/components/Instances';
import Models from '@/components/Models';
import ImportFile from '@/components/ImportFile';
import Racks from '@/components/Racks';


export default {
    
    components: {
        Users, 
        Instances,
        Models,
        ImportFile,
        Racks,
    },
    
}
</script>