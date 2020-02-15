<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
                <span class="headline">Instance Details</span>
            </v-card-title>
            <v-card-text>
                <v-row>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Host Name: </v-label>
                        <v-card-text> {{instance.hostname}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Rack Number: </v-label>
                        <v-card-text> {{instance.rack}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Rack Position: </v-label>
                        <v-card-text> {{instance.rackPosition}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Owner Username: </v-label>
                        <v-card-text v-if = "ownerPresent"> {{instance.owner.username}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Owner Display Name: </v-label>
                        <v-card-text v-if = "ownerPresent"> {{instance.owner.displayName}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Owner Email: </v-label>
                        <v-card-text v-if = "ownerPresent"> {{instance.owner.email}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Comment: </v-label>
                        <v-card-text> {{instance.comment}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model Vendor: </v-label>
                        <v-card-text> {{instance.model.vendor}} </v-card-text>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                        <v-label>Model: </v-label>
                        <router-link :to="{ name: 'model-details', params: { id: instance.model.id } }"> {{ instance.model.vendor }} </router-link>
                    </v-col>
                </v-row>

            </v-card-text>

            <v-spacer />

            <!--Back button to return to main page-->
            <v-spacer></v-spacer>
            <a href="javascript:history.go(-1)"> Go Back</a>

        </v-card>
    </div>
</template>

<script>
    export default {
        name: 'instance-details',
        inject: ['instanceRepository'],
        item: null,
        id: 0,
        ownerPresent: true,
        props: ['id'],
        data() {
            return {
                loading: false,
                instance: {
                    hostname: '',
                    rack: '',
                    rackPosition: '',
                    owner: {
                        id: 0, username: '', displayName: '', email: ''
                    },
                    comment: '',
                    model: { id: 0 }
                },
            };
        },
        created() {
            this.fetchInstance();
        },
        watch: {
            id: function () {
                this.fetchInstance();
            }
        },
        methods: {
            async fetchInstance() {
                if (!this.loading) this.loading = true;
                this.instance = await this.instanceRepository.find(this.id);
                this.loading = false;
                if (this.instance.owner == null) {
                    this.ownerPresent = false;
                }
            }
        }
    }
</script>