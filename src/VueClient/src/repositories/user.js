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
	}
};