const mockUsers = [
	{
		"id": 1,
		"username": "tbletsch",
		"firstName": "Tyler",
		"lastName": "Bletsch",
		"email": "Tyler.Bletsch@duke.edu"
	},
	{
		"id": 2,
		"username": "cpalms",
		"firstName": "Cannon",
        "lastName": "Palms",
		"email": "Cannon.Palms@duke.edu"
	},
	{
		"id": 3,
		"username": "bkeene",
		"firstName": "Brooke",
        "lastName": "Keene",
		"email": "Brooke.Keene@duke.edu"
	}
];

export default {
	find(id) {
		const item = mockUsers.find(o => o.id === id);
		if (item)
			return Promise.resolve(item);

        return Promise.reject();
    },
	list() {
		return Promise.resolve(mockUsers);
	},
	create(item) {
		if (mockUsers.find(o => o.id === item.id))
			return Promise.reject();

        mockUsers.push(item);
        return Promise.resolve();
    },
    update(item) {
		const index = mockUsers.findIndex(o => o.id === item.id);
		if (index < 0)
			return Promise.reject();

        mockUsers[index] = item;
        return Promise.resolve();
    },
    delete(item) {
		const index = mockUsers.findIndex(o => o.id === item.id);
        if (index < 0)
            return Promise.reject();

        mockUsers.splice(index, 1);
        return Promise.resolve();
    }
};