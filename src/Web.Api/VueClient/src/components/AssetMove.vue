<template>
    <v-card class="pa-3">
        <v-card-title v-if="type==='offline'"> Move Asset {{item.assetNumber}} to a Datacenter</v-card-title>
        <v-card-title v-if="type==='active'"> Move Asset {{item.assetNumber}} to Offline Storage</v-card-title>

        <MoveOptions v-on:selectedOfflineDatacenter="setDatacenter"
                     :editedItem="item"
                     :isBlade="false"
                     :type="type"
                     :offlineToDatacenter="offlineToDatacenter"></MoveOptions>

        <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color='primary' @click="moveToOffline">Move</v-btn>
            <v-btn color='primary' @click="closeMove">Cancel</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
    import MoveOptions from '@/components/MoveAssetOptions.vue';

    export default {
        props: ['item', 'type'],
        inject: ['assetRepository', 'rackRepository'],
        components: {
            MoveOptions
        },
        data() {
            return {
                assets: [],
                datacenterId: '',
                offlineToDatacenter: false,
            }
        },
        async created() {
            this.assets = await this.assetRepository.list();
            if (this.type === 'offline') {
                this.offlineToDatacenter = true;
            } else {
                this.offlineToDatacenter = false;
            }
        },
        methods: {
            async moveToOffline() {
                // Add backend call to move to offline assets (assetRepository.addOffline and also remove from active assets)
                console.log(this.datacenterId);
                var rack = await this.rackRepository.getOfflineRack(this.datacenterId);
                console.log(rack)

                var updateItem = this.item;
                updateItem.rackId = rack.id;

                console.log(updateItem);

                this.assetRepository.update(updateItem)
                this.$router.push({ name: 'assets', params: {type: this.type }})
;
            },
            async moveToActive() {
                this.assetRepository.update(this.item);
                this.$router.push({ name: 'assets', params: {type: this.type }})
            },
            closeMove() {
                this.$router.push({ name: 'assets', params: {type: this.type }})
            },
            async setDatacenter(id) {
                this.datacenterId = id;
            }
        }
    }
</script>