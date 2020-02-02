<template>
    <v-container>
        <div v-if="!loading">
            <v-card-title>Model {{id}} details</v-card-title>
            <!-- Contains the model data-->
            <v-row no-gutters v-for="(value, name, index) in model" v-bind:key="name">
                    <v-col>{{ index }}</v-col>
                    <v-col>{{ name }}</v-col>
                    <div v-if="name==='instances'">
                        <div v-for="(name) in value" v-bind:key="name">
                            <v-col>
                                <router-link :to="{ name: 'instance-details', params: { id: 1 } }">{{ name }}</router-link>
                            </v-col>
                        </div>
                    </div> 
                    <div v-else>
                        <v-col>{{ name }}</v-col>
                    </div>
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