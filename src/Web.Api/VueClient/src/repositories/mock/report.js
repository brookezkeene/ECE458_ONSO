/* eslint-disable no-unused-vars */

const mockRacks = [
    {
        "id": 1,
        "address": "B12",
        "rowLetter": "B",
        "rackNumber": 12,
        "instances": [
            {
                "id": 1,
                "model": {
                    "id": 1,
                    "vendor": "Dell",
                    "modelNumber": "R710",
                    "height": 4,
                    "displayColor": "#82E0AA",
                    "ethernetPorts": 2,
                    "powerPorts": 1,
                    "cpu": "Intel Xeon E5520 2.2GHz",
                    "memory": 32,
                    "storage": "2x500GB SSD RAID1",
                    "comment": "Retired offering, no new purchasing"
                },
                "hostname": "server1",
                "rack": "B12",
                "rackPosition": 4,
                "owner": {
                    "id": 1,
                    "username": "tbletsch",
                    "displayName": "Tyler Bletsch",
                    "email": "Tyler.Bletsch@duke.edu",
                },
                "comment": "Reserved for Palaemon project"
            },
            {
                "id": 2,
                "model": {
                    "id": 2,
                    "vendor": "Dell",
                    "modelNumber": "R700",
                    "height": 8,
                    "displayColor": "#C39BD3",
                    "ethernetPorts": 2,
                    "powerPorts": 1,
                    "cpu": "Intel Xeon E5520 2.2GHz",
                    "memory": 32,
                    "storage": "2x500GB SSD RAID1",
                    "comment": "This is my personal favorite!",
                    "instances": ["instance3", "instance4", "instance5"]
                },
                "hostname": "server2",
                "rack": "B12",
                "rackPosition": 12,
                "owner": {
                    "id": 1,
                    "username": "grodriguez",
                    "displayName": "Gaby Rodriguez-Florido",
                    "email": "gr64@duke.edu",
                },
                "comment": "Reserved for 458 project"
            },
        ]
    },
    {
        "id": 2,
        "address": "B13",
        "rowLetter": "B",
        "rackNumber": 13,
        "instances": [
            {
                "id": 1,
                "model": {
                    "id": 1,
                    "vendor": "Dell",
                    "modelNumber": "R710",
                    "height": 4,
                    "displayColor": "#82E0AA",
                    "ethernetPorts": 2,
                    "powerPorts": 1,
                    "cpu": "Intel Xeon E5520 2.2GHz",
                    "memory": 32,
                    "storage": "2x500GB SSD RAID1",
                    "comment": "Retired offering, no new purchasing"
                },
                "hostname": "server1",
                "rack": "B12",
                "rackPosition": 4,
                "owner": {
                    "id": 1,
                    "username": "bkeene",
                    "displayName": "Brooke Keene",
                    "email": "Brooke.Keene@duke.edu",
                },
                "comment": "Reserved for Palaemon project"
            },
            {
                "id": 2,
                "model": {
                    "id": 2,
                    "vendor": "Intel",
                    "modelNumber": "C500",
                    "height": 2,
                    "displayColor": "#C39BD3",
                    "ethernetPorts": 2,
                    "powerPorts": 1,
                    "cpu": "Intel Xeon E5520 2.2GHz",
                    "memory": 32,
                    "storage": "2x500GB SSD RAID1",
                    "comment": "This is my personal favorite!",
                    "instances": ["instance3", "instance4", "instance5"]
                },
                "hostname": "server2",
                "rack": "B12",
                "rackPosition": 12,
                "owner": {
                    "id": 1,
                    "username": "grodriguez",
                    "displayName": "Gaby Rodriguez-Florido",
                    "email": "gr64@duke.edu",
                },
                "comment": "Reserved for 458 project"
            },
        ]
    }
];

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

        var modelsPercent = {};
        Object.keys(models).map(function (key, index) {
            modelsPercent[key] = Math.round(models[key] / totalSpace * 100);
        });

        var ownersPercent = {};
        Object.keys(owners).map(function (key, index) {
            ownersPercent[key] = Math.round(owners[key] / totalSpace * 100);
        });

        res["vendors"] = vendors;
        res["vendorsPercent"] = vendorsPercent;
        res["models"] = models;
        res["modelsPercent"] = modelsPercent;
        res["owners"] = owners;
        res["ownersPercent"] = ownersPercent;

        return res;
    }
}
