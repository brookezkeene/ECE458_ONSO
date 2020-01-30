const RepositoryInterface = {
	find() {},
	list() {},
};

function bind(repositoryName, Interface) {
	return {
		...Object.keys(Interface).reduce((prev, method) => {
			const resolveableMethod = async (...args) => {
				const repository = await import(`./repositories/${repositoryName}`);
				return repository.default[method](...args);
			};
			return { ...prev, [method]: resolveableMethod };
		}, {}),
	};
}

export default {
	userRepository: bind('user', RepositoryInterface),
	modelRepository: bind('model', RepositoryInterface),
	rackRepository: bind('rack', RepositoryInterface),
	instanceRepository: bind('instance', RepositoryInterface),
};