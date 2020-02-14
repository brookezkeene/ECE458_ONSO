import userRepository from "./repositories/user";
import modelRepository from "./repositories/model";
import instanceRepository from "./repositories/mock/instance";
import rackRepository from "./repositories/rack";
import reportRepository from "./repositories/mock/report";
import auth from './auth';

export default {
	userRepository: userRepository,
	modelRepository: modelRepository,
	rackRepository: rackRepository,
	reportRepository: reportRepository,
	instanceRepository: instanceRepository,
	auth: auth
};