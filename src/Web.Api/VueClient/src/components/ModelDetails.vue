<template>
    <v-container>
        <div v-if="!loading">
            <!-- TODO: convert to simple table (maybe?) -->
            <v-row no-gutters v-for="(value, name, index) in model" v-bind:key="name">
                <v-col>{{ index }}</v-col>
                <v-col>{{ name }}</v-col>
                <v-col>{{ value }}</v-col>
            </v-row>

            <v-spacer />

            <!-- TODO: insert table of instances of this model -->
            <!-- <v-row no-gutters v-for="(instances) in model" v-bind:key="instances">
        <v-col>{{ instances }}</v-col>
    </v-row> -->
        </div>
    </v-container>
</template>

<script>
export default {
    name: 'model-details',
    inject: ['modelRepository'],
    props: ['id'],
    data () {
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
        id: function() {
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
}
</script>