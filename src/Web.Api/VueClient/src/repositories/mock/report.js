/* eslint-disable no-unused-vars */

const mockRacks = [
    {
        "id": 1,
        "address": "A1",
        "rowLetter": "A",
        "rackNumber": 1,
        "instances": [
            {
                "id": 1,
                "model": {
                    "id": 1,
                    "vendor": "Dell",
                    "modelNumber": "R710",
                    "height": 2,
                    "displayColor": "#82E0AA",
                    "ethernetPorts": 5,
                    "powerPorts": 2,
                    "cpu": "Intel Xeon E5520 2.2GHz",
                    "memory": 32,
                    "storage": "2x500GB SSD RAID1",
                    "comment": "Retired offering, no new purchasing"
                },
                "hostname": "server1",
                "rack": "A1",
                "rackPosition": 1,
                "owner": {
                    "id": 1,
                    "username": "jzhou",
                    "displayName": "Joyce Zhou",
                    "email": "jyz11@duke.edu",
                },
                "comment": "Reserved for Palaemon project"
            },
            {
                "id": 2,
                "model": {
                    "id": 2,
                    "vendor": "Cisco",
                    "modelNumber": "SG112",
                    "height": 1,
                    "displayColor": "#C39BD3",
                    "ethernetPorts": 24,
                    "powerPorts": 2,
                    "cpu": "Intel Xeon E5520 2.2GHz",
                    "memory": 32,
                    "storage": "2x500GB SSD RAID1",
                    "comment": "This is my personal favorite!",
                },
                "hostname": "switch1",
                "rack": "A1",
                "rackPosition": 4,
                "owner": {
                    "id": 1,
                    "username": "admin",
                    "displayName": "Admin User",
                    "email": "admin@test.com",
                },
                "comment": "Reserved for 458 project"
            },
            {
                "id": 3,
                "model": {
                    "id": 1,
                    "vendor": "Dell",
                    "modelNumber": "R630",
                    "height": 1,
                    "displayColor": "#82E0AA",
                    "ethernetPorts": 4,
                    "powerPorts": 2,
                    "cpu": "Intel Xeon E5520 2.2GHz",
                    "memory": 32,
                    "storage": "2x500GB SSD RAID1",
                    "comment": "Retired offering, no new purchasing"
                },
                "hostname": "server2",
                "rack": "A1",
                "rackPosition": 6,
                "owner": {
                    "id": 1,
                    "username": "bzk2",
                    "displayName": "Brooke Keene",
                    "email": "brooke.keene@duke.edu",
                },
                "comment": "Reserved for Duke project"
            },
        ]
    },
];

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
