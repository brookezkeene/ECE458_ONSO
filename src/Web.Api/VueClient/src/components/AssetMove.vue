<template>
    <v-card class="pa-3">
        <v-card-title v-if="type==='offline'"> Move Asset {{item.assetNumber}} to a Datacenter</v-card-title>
        <v-card-title v-if="type==='active'"> Move Asset {{item.assetNumber}} to Offline Storage</v-card-title>

        <SiteOptions :editedItem="assets"
                     :isBlade="false"
                     :type="type"></SiteOptions>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color='primary' @click="moveToOffline">Move</v-btn>
            <v-btn color='primary' @click="closeMove">Cancel</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import SiteOptions from '@/components/AssetEditSiteOptions.vue';

    export default {
        props: ['item', 'type'],
        inject: ['assetRepository'],
        components: {
            SiteOptions
        },
        data() {
            return {
                assets: [],
            }
        },
        async created() {
            this.assets = await this.assetRepository.list();
        },
        methods: {
            async moveToOffline() {
                // Add backend call to move to offline assets (assetRepository.addOffline and also remove from active assets)
            },
            async moveToActive() {
                // Add backend call to move to active assets (assetRepository.add() and also need to remove from offline assets)
            },
            closeMove() {
                this.$router.push({ name: 'assets', params: {type: this.type }})
            }
        }
    }
</script>