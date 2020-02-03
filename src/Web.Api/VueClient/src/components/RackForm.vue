<template>
    <v-container align="center" justify="center">
        <v-card max-width="50%">
            <v-card-title>Rack Range Selection</v-card-title>

            <v-card-text class="justify-center">
                <form>
                    <v-text-field v-model="range.start" label="Start"></v-text-field>
                    <v-text-field v-model="range.end" label="End"></v-text-field>

                    <v-btn class="mr-4" color="primary" @click="viewDiagram">View</v-btn>
                    <v-btn class="mr-4" color="primary" @click="createInRange">Create</v-btn>
                    <v-btn class="mr-4" color="primary" @click="deleteInRange">Delete</v-btn>
                </form>
            </v-card-text>
        </v-card>
    </v-container>
</template>

<script>
export default {
    name: 'rack-form',
    inject: ['rackRepository'],
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
        },
        deleteInRange() {
            this.rackRepository.deleteInRange(this.range.start, this.range.end)
                .then(() => {
                    // indicate success
                })
                .catch(() => {
                    // indicate failure
                })
        }
    }
}
</script>