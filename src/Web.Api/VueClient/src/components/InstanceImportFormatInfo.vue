<template>
    <div v-if="!loading">
        <v-card>
            <v-card-title>
                Insert info about importing the instance format!
            </v-card-title>
        </v-card>
    </div>
</template>

<script>
export default {
    name: 'model-input-format-info',
    data () {
        return {
            loading: false,
        };
    }
}   
</script>