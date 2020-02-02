<template>
    <v-container>
        <div v-if="!loading">
            <v-card-title>Model {{id}} details</v-card-title>
            <!-- Contains the model data-->
            <v-row>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Model: </v-label>
                    <v-card-text> {{model.vendor}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Model Number: </v-label>
                    <v-card-text> {{model.modelNumber}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Height: </v-label>
                    <v-card-text> {{model.height}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Display Color: </v-label>
                    <v-card-text> {{model.displayColor}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Ethernet Ports: </v-label>
                    <v-card-text> {{model.ethernetPorts}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Power Ports: </v-label>
                    <v-card-text> {{model.powerPorts}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>CPU: </v-label>
                    <v-card-text> {{model.cpu}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Memory: </v-label>
                    <v-card-text> {{model.memory}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Storage: </v-label>
                    <v-card-text> {{model.storage}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Comment: </v-label>
                    <v-card-text> {{model.comment}} </v-card-text>
                </v-col>

            </v-row>
            <v-label>Instances: </v-label>
            <v-row no-gutters v-for="(value, name) in model.instances" v-bind:key="name">
                <v-label>Host Name: </v-label>
                <router-link :to="{ name: 'instance-details', params: { id: value.id } }">{{ value.hostname }}</router-link>

            </v-row>

            <v-spacer />

            <!--Back button to return to main page-->
            <a href="javascript:history.go(-1)"> Go Back</a>
        </div>
    </v-container>
</template>

<script>

    export default {
        name: 'model-details',
        inject: ['modelRepository'],
        props: ['id'],
        data() {
            return {
                loading: false,
                model: {
                    vendor: '',
                    modelNumber: 'foo',
                }
            };
        },
        created() {
            this.fetchModel();
        },
        watch: {
            id: function () {
                this.fetchModel();
            }
        },
        methods: {
            async fetchModel() {
                if (!this.loading) this.loading = true;
                this.model = await this.modelRepository.find(this.id);
                this.loading = false;
            }
        }
    }</script>