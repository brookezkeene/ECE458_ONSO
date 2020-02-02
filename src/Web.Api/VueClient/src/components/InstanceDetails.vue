<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
            <span class="headline">Instance {{id}} Details</span>
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
                    <v-label>Owner ID: </v-label>
                    <v-card-text> {{instance.owner.id}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Owner Username: </v-label>
                    <v-card-text> {{instance.owner.username}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Owner Display Name: </v-label>
                    <v-card-text> {{instance.owner.displayName}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Owner Email: </v-label>
                    <v-card-text> {{instance.owner.email}} </v-card-text>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-label>Comment: </v-label>
                    <v-card-text> {{instance.comment}} </v-card-text>
                </v-col>
            </v-row>

</v-card-text>
        </v-card>
    </div>
</template>

<script>
export default {
    name: 'instance-details',
    inject: ['instanceRepository'],
    item: null,
    id : 0,
    props: ['id'],
    data () {
        return {
            loading: false,
            instance: {
                hostname: '',
                rack: '',
                rackPosition: '',
                owner: { id:0, username:'', displayName:'', email:''
                    },
                comment: '',
                model: {id:0}
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
        }
    }
}
</script>