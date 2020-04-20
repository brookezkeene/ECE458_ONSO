<template>
    <v-card flat width="20%">
        <v-container flat align-center media="print" id="print">
            <div class="avery">
                <!--       <div id="controls">
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
                -->
                <Label v-for="(asset, index) in assets"
                       :key="index"
                       :asset="asset" />
            </div>
        </v-container>
        <v-card-actions>
            <v-btn class="noPrint"
                   color="primary" @click="print">Print Labels</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import Label from './Label';

    export default {
        name: 'asset-labels',
        props: ['assets'],
        components: {
            Label,
        },
        data() {
            return {
                // for testing entire printing layout
                //    assets: [
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //        { assetNumber: 100000 },
                //    ]
            };
        },
        watch: {
        },
        methods: {
            print() {
                window.print();
            },
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="scss" scoped>
    .label {
        font-weight: bold;
        margin-right: 0.5em;
    }

    .avery {
        width: 750px;
    }

    @media print {
        @page {
            size: 8.5in 11in portrait;
            margin: 12.7mm 7.874mm 10.668mm 9.906mm;
        }

        .body {
            margin: 12.7mm 7.874mm 10.668mm 9.906mm;
        }

        .noPrint {
            display: none;
        }
    }
</style>