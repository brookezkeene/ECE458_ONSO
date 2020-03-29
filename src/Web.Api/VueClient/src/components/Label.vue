<template>
    <div class="label">
        <svg class="barcode"
             :jsbarcode-format="CODE128C"
             :jsbarcode-value="value"
             jsbarcode-textmargin="0"
             jsbarcode-fontoptions="bold"
             jsbarcode-height="15"
             jsbarcode-width="2"
             jsbarcode-margins="2"
             :jsbarcode-text="text"></svg>
    </div>
</template>

<script>
    //import bwipjs from 'bwip-js';
    import JsBarcode from 'jsbarcode';

    export default {
        name: 'Label',
        props: {
            value: Number,
            text: String,
        },
        data() {
            return {
                canvas: null,
                generatedImage: null,
                options: {
                    bcid: 'datamatrixrectangular',
                    scale: 2,
                    includetext: false,
                }
            };
        },
        mounted() {
            JsBarcode(".barcode").init();
        },
        methods: {
        },

    }
</script>

<style lang="scss" scoped>
    .label {
        height: 48px;
        width: 168px;
        padding: 0px;
        margin-right: 10px;
        text-align: center;
        display: inline-block;
        position: relative;
        vertical-align: middle;
        border: 1px solid black;
        border-radius: 5px;
    }

    span {
        display: inline-block;
    }

    @media print {
        .label {
            border: 0;
        }
    }
</style>