import userRepository from "./repositories/user";
import modelRepository from "./repositories/model";
import instanceRepository from "./repositories/mock/instance";
import rackRepository from "./repositories/mock/rack";
import auth from './auth';

export default {
	userRepository: userRepository,
	modelRepository: modelRepository,
	rackRepository: rackRepository,
	instanceRepository: instanceRepository,
	auth: auth
};