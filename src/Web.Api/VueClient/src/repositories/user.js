const mockUsers = [
	{
		"id": 1,
		"username": "tbletsch",
		"displayName": "Tyler Bletsch",
		"email": "Tyler.Bletsch@duke.edu"
	},
	{
		"id": 2,
		"username": "cpalms",
		"displayName": "Cannon Palms",
		"email": "Cannon.Palms@duke.edu"
	},
	{
		"id": 3,
		"username": "bkeene",
		"displayName": "Brooke Keene",
		"email": "Brooke.Keene@duke.edu"
	}
];

export default {
	find(id) {
		return Promise.resolve(mockUsers.find(o => o.id == id));

	},
	list() {
		return Promise.resolve(mockUsers);
	},
    create(item) {
        mockUsers.push(item);
        return Promise.resolve();
    },
    update(item) {
		const index = mockUsers.find(o => o.id == item.id);
        mockUsers[index] = item;
        return Promise.resolve();
    },
    delete(item) {
		const index = mockUsers.find(o => o.id == item.id);
        mockUsers.splice(index, 1);
        return Promise.resolve();
    }
};