<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
            <span class="headline">Instance {{id}} Details</span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-row no-gutters v-for="(value, name, index) in instance" v-bind:key="index">
                        <v-col>{{ index }}</v-col>
                        <v-col>{{ name }}</v-col>
                        <v-col>{{ value }}</v-col>
                    </v-row>
                </v-container>
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
                id: 0,
                hostname: '',
                rack: '',
                rackPosition: '',
                owner: '',
                comment: '',
                model: Object
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