import userRepository from './repositories/user';
import modelRepository from './repositories/model';
import instanceRepository from './repositories/instance';
import rackRepository from './repositories/rack';
import auth from './auth';

export default {
	userRepository: userRepository,
	modelRepository: modelRepository,
	rackRepository: rackRepository,
	instanceRepository: instanceRepository,
	auth: auth
};