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
	}
};