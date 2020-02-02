const mockRacks = [
	{
		"id": 1,
		"address": "B12",
		"position": {
			"row": "B",
			"column": 12,
		}
	},
	{
		"id": 2,
		"address": "B13",
		"position": {
			"row": "B",
			"column": 13
		}
	}
];

export default {
	find(id) {
		return mockRacks.filter(o => { return o.id == id; });
	},
	list() {
		return mockRacks;
	},
	findInRange() {
        return Promise.resolve([
                {
                    rowLetter: 'A',
                    rackNumber: 1,
                    address: 'A1',
                    instances: [
                        {
                            id: 1,
                            model: {
                                vendor: 'foo-vendor',
                                modelNumber: 'foo-model',
                                displayColor: '#82E0AA',
                                height: 4
                            },
                            hostname: 'server1',
                            rackPosition: 1
                        },
                        {
                            id: 2,
                            hostname: 'server2',
                            model: {
                                vendor: 'foo-vendor',
                                modelNumber: 'foo-model',
                                displayColor: '#82E0AA',
                                height: 4
                            },
                            rackPosition: 12
                        }
                    ]
                }
            ]);
    }
};