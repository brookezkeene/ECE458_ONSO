<template>
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
               :asset="asset"/>
    </div>
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
            };
        },
        watch: {
        },
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="scss" scoped>
    label {
        font-weight: bold;
        margin-right: 0.5em;
    }

    @media print {
        @page {
            size: A4 portrait;
            margin: 12.7mm 7.874mm 10.668mm 9.906mm;
        }
        body {
            margin: 12.7mm 7.874mm 10.668mm 9.906mm;
        }
    }
</style>