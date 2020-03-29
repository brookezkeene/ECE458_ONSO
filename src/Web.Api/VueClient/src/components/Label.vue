<template>
    <div class="label">
        <svg class="barcode"
             jsbarcode-format="CODE128C"
             :jsbarcode-value="value"
             jsbarcode-height="28"
             jsbarcode-width="2"
             :jsbarcode-text="text"
             jsbarcode-fontoptions="bold"
             jsbarcode-fontsize="10"
             jsbarcode-textmargin="0"
             jsbarcode-margintop="3"
             jsbarcode-marginbottom="0"
             ></svg>
    </div>
</template>

<script>
    import JsBarcode from 'jsbarcode';

    export default {
        name: 'Label',
        props: ['asset'],
        data() {
            return {
                canvas: null,
                generatedImage: null,
            };
        },
        computed: {
            value() {
                return this.asset.assetNumber
            },
            text() {
                return "Hyposoft " + this.asset.assetNumber
            }
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
        padding: 0px !important; 
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