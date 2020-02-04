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
                    "modelNumber": "R710",
                    "height": 4,
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
                    "username": "tbletsch",
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
                    "modelNumber": "R710",
                    "height": 4,
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
                    "username": "tbletsch",
                    "displayName": "Gaby Rodriguez-Florido",
                    "email": "gr64@duke.edu",
                },
                "comment": "Reserved for 458 project"
            },
        ]
	}
];

const splitAddress = (address) => {
    if (!validAddress(address))
        return {};
    const groups = address.match(/^([a-zA-Z])(\d+)$/);
    return { rowLetter: groups[1].toUpperCase(), rackNumber: parseInt(groups[2]) }
}

const validAddress = (address) => {
    return /^[a-zA-Z]\d+$/.test(address);
}

const uuidv4 = () => {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}



export default {
    find(id) {
        const item = mockRacks.find(o => o.id === id);
        if (item)
            return Promise.resolve(item);

        return Promise.reject();
    },
    list() {
        return Promise.resolve(mockRacks);
    },
    findInRange(start, end) {
        if (!(validAddress(start) && validAddress(end))) {
            return Promise.reject();
        }
            
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);
        const racksInRange = mockRacks.filter(o => {
            return o.rowLetter >= startRow &&
                o.rowLetter <= endRow &&
                o.rackNumber >= startCol &&
                o.rackNumber <= endCol;
        });
        // this array will be populated, so we add the property to the mocked response, at the very least
        //racksInRange.forEach(o => o.instances = []);
        return Promise.resolve(racksInRange);
    },
    createInRange(start, end) {
        if (!(validAddress(start) && validAddress(end)))
            return Promise.reject();
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);

        for (var r = startRow.charCodeAt(0); r <= endRow.charCodeAt(0); r++) {
            const row = String.fromCharCode(r);
            for (var col = startCol; col <= endCol; col++) {
                if (!mockRacks.find(o => o.rowLetter == row && o.rackNumber == col))
                    mockRacks.push({
                        id: uuidv4(),
                        address: `${row}${col}`,
                        rowLetter: row,
                        rackNumber: col
                    });
            }
        }
        return Promise.resolve();
    },
    deleteInRange(start, end) {
        if (!(validAddress(start) && validAddress(end)))
            return Promise.reject();
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);

        for (var r = startRow.charCodeAt(0); r <= endRow.charCodeAt(0); r++) {
            const row = String.fromCharCode(r);
            for (var col = startCol; col <= endCol; col++) {
                const index = mockRacks.find(o => o.rowLetter == row && o.rackNumber == col);
                mockRacks.splice(index, 1);
            }
        }
        return Promise.resolve();
    },

    validAddress: validAddress,
    splitAddress: splitAddress
};