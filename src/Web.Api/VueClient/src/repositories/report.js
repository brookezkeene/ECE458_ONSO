/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/reports';
export default {
    generate() {
        var racks = axios.get(`${resource}/usage`)
            .then(response => response.data)
            .catch(error => {
                console.log(error)
            });

        // calculate statistics for report
        var numRacks = 0;
        var rackHeight = 42;
        var usedSpace = 0; // freeSpace = 42 - usedSpace
        var vendors = {};
        var models = {};
        var owners = {};

        racks.forEach(rack => {
            numRacks++;
            rack.instances.forEach(asset => {
                console.log(asset)
                var model = asset.model;
                // calculate free vs. used space
                usedSpace += model.height;
                // calculate allocated per vendor
                if (typeof vendors[model.vendor] == "undefined") {
                    vendors[model.vendor] = model.height;
                } else {
                    vendors[model.vendor] += model.height;
                }
                // calculate allocated per model
                if (typeof models[model.modelNumber] == "undefined") {
                    models[model.modelNumber] = model.height;
                } else {
                    models[model.modelNumber] += model.height;
                }
                // calculate allocated per owner
                if (typeof owners[asset.owner.username] == "undefined") {
                    owners[asset.owner.username] = model.height;
                } else {
                    owners[asset.owner.username] += model.height;
                }
            });
        });

        // make new object and return
        var res = {};
        res["usedSpace"] = usedSpace;
        res["freeSpace"] = rackHeight * numRacks - usedSpace;
        res["vendors"] = vendors;
        res["models"] = models;
        res["owners"] = owners;

        return res;
    }
}