<template>
    <div class="avery">
        <div id="controls">
            <h1>Label Generator</h1>
            <hr>
            <h2>Labels</h2>
            <section>
                <p>
                    Place each label on a separate line. It should be fine to copy-paste from Excel...
                </p>
                <p>
                    <textarea v-model="barcodeInput" cols="30" rows="25"></textarea>
                </p>
            </section>
            <hr>
            <h2>Press Ctrl+P when you're ready to print these labels!</h2>
            <section>
                <p>This app was mainly tested in <strong>Chrome</strong>! It'll probably work in Firefox too...</p>
                <p>When printing, try to use the same page margins as the <strong>Avery 5167 label template</strong> (clockwise from top):</p>
                <ul>
                    <li><strong>Top:</strong> 0.5"</li>
                    <li><strong>Right:</strong> 0.31"</li>
                    <li><strong>Bottom:</strong> 0.42"</li>
                    <li><strong>Left:</strong> 0.39"</li>
                </ul>
            </section>
            
        </div>
        <Label v-for="(code, index) in barcodes"
               :key="index"
               :value="code"
               :text="code"/>
    </div>
</template>

<script>
    import Label from './Label';

    export default {
        name: 'Avery',
        components: {
            Label,
        },
        data() {
            return {
                barcodeInput: '',
                barcodes: [],
                settings: {
                    height: 48,
                    width: 166,
                    padding: 2,
                    rightMargin: 20,
                    barcodeHeightMM: 5,
                    cornerRadius: 5,
                },
            };
        },
        watch: {
            barcodeInput: function (newVal) {
                if (newVal) {
                    this.barcodes = newVal.split('\n');
                } else {
                    this.barcodes = [];
                }
            },
        },
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="scss" scoped>
    #controls {
        padding: 1em;
    }

    p, hr {
        margin: 1em 0;
    }

    ul {
        margin: 1em;
    }

    label {
        font-weight: bold;
        margin-right: 0.5em;
    }

    input {
        display: block;
        font-size: 1em;
    }

    @media print {
        @page {
            size: A4 portrait;
            margin: 0mm 0mm 0mm 0mm;
        }
        body {
            margin: 0px
        }
        #controls {
            display: none;
        }
    }
</style>