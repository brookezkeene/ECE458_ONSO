<template>
    <v-container>
        <v-card flat>
            <v-card-title>
                <span class="headline">Range selection</span>
            </v-card-title>

            <v-card-text>
                <form>
                    <v-text-field v-model="range.start" label="Start"></v-text-field>
                    <v-text-field v-model="range.end" label="End"></v-text-field>

                    <v-btn class="mr-4" @click="viewDiagram">View</v-btn>
                    <v-btn @click="createInRange">Create</v-btn>
                </form>
            </v-card-text>
        </v-card>
    </v-container>
</template>

<script>
export default {
    name: 'rack-form',
    inject: ['rackRepository'],
    item: null,
    data: () => ({
        range: {
            start: '',
            end: ''
        }
    }),
    methods: {
        viewDiagram() {
            this.$router.push({ name: "RackDiagram", query: { start: this.range.start, end: this.range.end } });
        },
        createInRange() {
            this.rackRepository.createInRange(this.range.start, this.range.end)
                .then(() => {
                    // indicate success
                })
                .catch(() => {
                    // indicate failure
                });
        }
    }
}
</script>