const mockInstances = [
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
		"hostname": "server9",
		"rack": "B12",
		"rackPosition": 5,
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
			"vendor": "Lenovo",
			"modelNumber": "Foobar",
			"height": 3,
			"displayColor": "#C39BD3",
            "ethernetPorts": 2,
			"powerPorts": 1,
			"cpu": "Intel Xeon E5520 2.2GHz",
			"memory": 32,
			"storage": "2x500GB SSD RAID1",
			"comment": "This is my personal favorite!",
			"instances": ["instance3", "instance4", "instance5"]
		},
		"hostname": "server10",
		"rack": "B15",
		"rackPosition": 9,
		"owner": {
			"id": 1,
			"username": "tbletsch",
			"displayName": "Gaby Rodriguez-Florido",
			"email": "gr64@duke.edu",
		},
		"comment": "Reserved for 458 project"
	},
	{
		"id": 3,
		"model": {
			"id": 2,
			"vendor": "Lenovo",
			"modelNumber": "Foobar",
			"height": 4,
			"displayColor": "#C39BD3",
			"ethernetPorts": 2,
			"powerPorts": 1,
			"cpu": "Intel Xeon E5520 2.2GHz",
			"memory": 32,
			"storage": "2x500GB SSD RAID1",
			"comment": "Retired offering, no new purchasing"
		},
		"hostname": "server5",
		"rack": "B13",
		"rackPosition": 1,
		"owner": {
			"id": 2,
			"username": "cpalms",
			"displayName": "Cannon Palms",
			"email": "Cannon.Palms@duke.edu",
		},
		"comment": "Reserved for Palaemon project"
	}
];

export default {
    find(id) {
		const item = mockInstances.find(o => o.id === id);
        if (item)
            return Promise.resolve(item);

        return Promise.reject();
    },
    list() {
		return Promise.resolve(mockInstances);
    },
    create(item) {
		if (mockInstances.find(o => o.id === item.id))
            return Promise.reject();

        mockInstances.push(item);
        return Promise.resolve();
    },
    update(item) {
		const index = mockInstances.findIndex(o => o.id === item.id);
        if (index < 0)
            return Promise.reject();

        mockInstances[index] = item;
        return Promise.resolve();
    },
    delete(item) {
		const index = mockInstances.findIndex(o => o.id === item.id);
        if (index < 0)
            return Promise.reject();

        mockInstances.splice(index, 1);
        return Promise.resolve();
    }
};