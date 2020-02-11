/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/reports';
export default {
    generate() {
        // calculate statistics for report
        var numRacks = 0;
        var usedSpace = 0; // freeSpace = 42 - usedSpace
        var vendors = {};
        var models = {};
        var owners = {};

        mockRacks.forEach(rack => {
            numRacks++;
            rack.instances.forEach(asset => {
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
        var rackHeight = 42;
        var totalSpace = rackHeight * numRacks;
        var res = {};
        res["usedSpace"] = Math.round(usedSpace / totalSpace * 100);
        res["freeSpace"] = Math.round((totalSpace - usedSpace) / totalSpace * 100);

        // calculate percentages for all fields
        var vendorsPercent = {};
        Object.keys(vendors).map(function (key, index) {
            vendorsPercent[key] = Math.round(vendors[key] / totalSpace * 100);
        });
        vendorsPercent = sortProperties(vendorsPercent);

        var modelsPercent = {};
        Object.keys(models).map(function (key, index) {
            modelsPercent[key] = Math.round(models[key] / totalSpace * 100);
        });
        modelsPercent = sortProperties(modelsPercent);

        var ownersPercent = {};
        Object.keys(owners).map(function (key, index) {
            ownersPercent[key] = Math.round(owners[key] / totalSpace * 100);
        });
        ownersPercent = sortProperties(ownersPercent);

        vendors = sortProperties(vendors);
        models = sortProperties(models);
        owners = sortProperties(owners);

        res["vendors"] = vendors;
        res["vendorsPercent"] = vendorsPercent;
        res["models"] = models;
        res["modelsPercent"] = modelsPercent;
        res["owners"] = owners;
        res["ownersPercent"] = ownersPercent;

        return res;
    }
}