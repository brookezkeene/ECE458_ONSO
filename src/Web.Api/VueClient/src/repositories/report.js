/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/reports';

function sortProperties(obj) {
    var sortable = [];
    for (var key in obj)
        if (obj.hasOwnProperty(key))
            sortable.push([key, obj[key]]);

    sortable.sort(function (a, b) {
        return b[1] - a[1];
    });
    return sortable; // array in format [ [ key1, val1 ], [ key2, val2 ], ... ]
}

export default {
    calculateStatistics(racks) {
        // calculate statistics for report
        var numRacks = 0;
        var usedSpace = 0; // freeSpace = 42 - usedSpace
        var vendors = {};
        var models = {};
        var owners = {};

        racks.forEach(rack => {
            numRacks++;
            rack.assets.forEach(asset => {
                // calculate free vs. used space
                usedSpace += asset.height;
                // calculate allocated per vendor
                if (typeof vendors[asset.vendor] == "undefined") {
                    vendors[asset.vendor] = asset.height;
                } else {
                    vendors[asset.vendor] += asset.height;
                }
                // calculate allocated per model
                if (typeof models[asset.modelNumber] == "undefined") {
                    models[asset.modelNumber] = asset.height;
                } else {
                    models[asset.modelNumber] += asset.height;
                }
                // calculate allocated per owner
                if (typeof owners[asset.owner] == "undefined") {
                    owners[asset.owner] = asset.height;
                } else {
                    owners[asset.owner] += asset.height;
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