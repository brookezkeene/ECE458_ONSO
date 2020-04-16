<template>
    <div v-resize-text>
        <v-card flat>
            <v-card-title>Scan Asset Label</v-card-title>
            <v-btn @click="scan=!scan" color="primary" class="center mb-2" :depressed="scan">Click to scan</v-btn>
            <v-quagga v-if="scan===true" :onDetected="logIt" :readerSize="readerSize"></v-quagga>
        </v-card>
    </div>
</template>

<script>
    import Vue from 'vue'
    import VueQuagga from 'vue-quaggajs';

    // register component 'v-quagga'
    Vue.use(VueQuagga);

    export default {
        name: 'VueBarcodeTest',
        inject: ['assetRepository'],
        data() {
            return {
                readerSize: {
                    width: 640,
                    height: 480
                },
                detecteds: [],
                scan: false
            }
        },
        methods: {
            async logIt(data) {
                /*eslint-disable*/
                console.log('detected:', data);
                console.log('asset:', data.codeResult.code);
                var asset = await this.assetRepository.findByNumber(data.codeResult.code);
                console.log(asset);
                this.$router.push({ name: 'asset-details', params: { id: asset.id, type: asset.type} });
            }

        }
    }
</script>