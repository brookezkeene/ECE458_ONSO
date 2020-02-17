const mockDatacenters = [
	{
		"id": 1,
		"abbreviation": "RTP1",
		"name": "Research Triangle Park lab 1"
	},
	{
		"id": 2,
		"abbreviation": "RTP2",
		"name": "Research Triangle Park lab 2"
	},
	{
		"id": 3,
		"abbreviation": "SV1",
		"name": "Silicon Valley lab 1"
	},
];

export default {
	find(id) {
		const item = mockDatacenters.find(o => o.id === id);
		if (item)
			return Promise.resolve(item);

		return Promise.reject();
	},
	list() {
		return Promise.resolve(mockDatacenters);
	},
	create(item) {
		if (mockDatacenters.find(o => o.id === item.id))
			return Promise.reject();

		mockDatacenters.push(item);
		return Promise.resolve();
	},
	update(item) {
		const index = mockDatacenters.findIndex(o => o.id === item.id);
		if (index < 0)
			return Promise.reject();

		mockDatacenters[index] = item;
		return Promise.resolve();
	},
	delete(item) {
		const index = mockDatacenters.findIndex(o => o.id === item.id);
		if (index < 0)
			return Promise.reject();

		mockDatacenters.splice(index, 1);
		return Promise.resolve();
	}
};