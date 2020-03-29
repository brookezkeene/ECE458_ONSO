<template>
    <div class="avery">
        <v-layout>
            <div v-for="(label, index) in barcodes" :key="index">
                <p>{{asset.hostname}}</p>
            </div>

        </v-layout>
    </div>
</template>

<script>
//import Label from './Label';
    import JsBarcode from 'jsbarcode'

export default {
  name: 'asset-labels',
    props: ['assets'],
    components: {
    //Label,
    },
    data() {
        return {
            barcodes: [],
            settings: {
            height: 48,
            width: 166,
            padding: 2,
            rightMargin: 20,
            barcodeHeightMM: 5,
            cornerRadius: 5,
            }
        };
        },
    created() {
        this.initialize();
    },
    methods: {
        /*eslint-disable*/
        initialize() {
            for (var i = 0; i < assets.length; i++) {
                console.log(assets[i]);
                var barcode = { format: 'CODE128C', value: 100000 + i };
                this.barcodes.push(barcode);
            }
        }
     },
    mounted () {
        JsBarcode(".barcode").init();
      }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="scss" scoped>
    label {
        font-weight: bold;
        margin-right: 0.5em;
    }

    input {
        display: block;
        font-size: 1em;
    }

    @media print {
        #controls {
            display: none;
        }
    }
</style>