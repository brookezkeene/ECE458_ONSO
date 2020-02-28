<template>
    <v-container max-width ="75%" max-height="400px">
        <d3-network :net-nodes="nodes" 
                    :net-links="links" 
                    :options="options"
                    @node-click="nodeClicked"/>
    </v-container>
</template>

<style src="vue-d3-network/dist/vue-d3-network.css"></style>

<script>
    import D3Network from 'vue-d3-network'
    import networkNeighborhood from '@/networkNeighborhood';


    export default {
        name: 'network-neighborhood',
        components: {
            D3Network
        },
        inject: ['assetRepository'],
        props: ['id'],
        data () {
            return {
                nodes: [],
                links: [],
                options: {
                    force: 3000,
                    forces: {
                        X: 0,
                        Y: 0,
                        //ManyBody: true,
                        //Link: false,
                    },
                    nodeSize: 30,
                    nodeLabels: true,
                    linkWidth: 5,
                },
                loading: true,
            }
        },
        async created() {
            this.initialize();
        },  
        methods: {
            async initialize() {
                /* eslint-disable no-unused-vars, no-console */
                networkNeighborhood.createGraph(this.id).then((response) =>
                {
                    this.nodes = response.nodes;
                    this.links = response.links;
                });
                console.log(this.nodes);
                console.log(this.links);
            },
            nodeClicked(event, node) {
                //this.$router.push({ name: 'asset-details', params: { id: node.id } })
                this.$emit('click', node.id);
            },
            /*forcesFunc(sim) {
                sim.force('link', d3.forceLink(this.links).distance(50))

                return sim
            },*/
            /*lcb (link) {
                link._svgAttrs = { 'pathlength': 50}
                return link
            },*/
            /*tick() {
                link.attr("x1", function(d) { return d.source.x; })
                    .attr("y1", function(d) { return d.source.y; })
                    .attr("x2", function(d) { return d.target.x; })
                    .attr("y2", function(d) { return d.target.y; });

                node.attr("cx", function(d) { return d.x; })
                    .attr("cy", function(d) { return d.y; });
            }*/
        }
    }
</script>