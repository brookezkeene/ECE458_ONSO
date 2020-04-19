/* eslint-disable no-unused-vars, no-console */
import assetRepository from '@/repositories/asset';

export default {
    async createGraph(assetId, changePlanId) {
        var graphNodes = new Array(0);
        var graphLinks = new Array(0);
        const fakeNodes = [
            { id: 1, name: 'server1' },
            { id: 2, name: 'server2' },
            { id: 3, name: 'switch1' },
            { id: 4, },
            { id: 5 },
            { id: 6 },
            { id: 7 },
            { id: 8 },
            { id: 9 },
        ];
        const fakeLinks = [
            { sid: 1, tid: 2 },
            { sid: 2, tid: 8 },
            { sid: 3, tid: 4 },
            { sid: 4, tid: 5 },
            { sid: 5, tid: 6 },
            { sid: 7, tid: 8 },
            { sid: 5, tid: 8 },
            { sid: 3, tid: 8 },
            { sid: 7, tid: 9 }
        ];

        // Create Nodes and Links
        var seen = new Array();
        // Start with main asset
        var asset = await assetRepository.find(assetId, changePlanId);
        graphNodes.push({ id: asset.id, name: asset.hostname });
        seen.push(asset.id);
        // Find assets 1 level of separation away
        var i, j;
        for (i = 0; i < asset.networkPorts.length; i++) {
            if (asset.networkPorts[i].connectedPort != null) {
                var asset1 = await assetRepository.find(asset.networkPorts[i].connectedPort.assetId);
                if (!seen.includes(asset1.id)) {
                    seen.push(asset1.id);

                    if (asset1 != null) {
                        // add new node
                        graphNodes.push({ id: asset1.id, name: asset1.hostname });
                        // add new connection
                        graphLinks.push({ sid: asset.id, tid: asset1.id });

                        // Find assets 2 levels of separation away
                        for (j = 0; j < asset1.networkPorts.length; j++) {
                            if (asset1.networkPorts[j].connectedPort != null) {
                                var asset2 = await assetRepository.find(asset1.networkPorts[j].connectedPort.assetId);
                                if (!seen.includes(asset2.id)) {
                                    seen.push(asset2.id);

                                    if (asset2 != null) {
                                        // add new node
                                        graphNodes.push({ id: asset2.id, name: asset2.hostname });
                                        // add new connection
                                        graphLinks.push({ sid: asset1.id, tid: asset2.id });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        var response = {
            nodes: graphNodes,
            links: graphLinks,
        };
        console.log(response);
        return response;
    },
}