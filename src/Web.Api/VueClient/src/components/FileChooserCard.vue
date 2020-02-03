<template>
    <div v-if="!loading">
        <v-stepper v-model="step">
            <v-stepper-header>
                <v-stepper-step step="1" :complete="step > 1">Upload File</v-stepper-step>
                <v-divider></v-divider>
                <v-stepper-step step="2" :complete="step > 2">Review Warnings & Errors</v-stepper-step>
                <v-divider></v-divider>
                <v-stepper-step step="3">Finalize</v-stepper-step>
            </v-stepper-header>

            <v-stepper-items>
                <v-stepper-content step="1">
                    <v-card flat>
                        <v-file-input></v-file-input>
                    </v-card>
                    <v-btn color="primary" @click="step = 2">Continue</v-btn>
                </v-stepper-content>

                <v-stepper-content step="2">
                    <v-card flat>

                    </v-card>
                    <v-btn flat @click.native="step = 1">Previous</v-btn>
                    <v-btn color="primary" @click="step = 3">Continue</v-btn>
                </v-stepper-content>

                <v-stepper-content step="3">
                    <v-card flat>

                    </v-card>
                    <v-btn flat @click="step = 2">Previous</v-btn>
                    <v-btn color="primary" @click.prevent="close">Save</v-btn>
                </v-stepper-content>
            </v-stepper-items>
        </v-stepper>
    </div>
</template>

<script>
export default {
    name: 'file-chooser-card',
    props: ['filechoosercard'],
    data () {
        return {
            loading: false,
            step:1,
        };
    },
    methods: {
        close() {
            alert('Data Successfully Imported.');
            this.$emit('close-file-chooser');
            this.step = 1;
        }
    }
}   
</script>