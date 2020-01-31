<template>
    <div v-if="!loading">
        <v-card>
            <v-card-title>
                Choose File to Import!
            </v-card-title>
            <v-file-input>
            </v-file-input>
        </v-card>
    </div>
</template>

<script>
export default {
    name: 'file-chooser-card',
    data () {
        return {
            loading: false,
        };
    }
}   
</script>