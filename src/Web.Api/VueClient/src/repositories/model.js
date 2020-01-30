const mockModels = [
	{
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
		"comment": "Retired offering, no new purchasing",
		// "instances": "hello"
	},
	{
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
		// "instances": "hello2"
	}
];

export default {
	find(id) {
		return Promise.resolve(mockModels.find(o => o.id == id));
	},
	list() {
		return mockModels;
	}
};