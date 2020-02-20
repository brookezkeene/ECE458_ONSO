import userRepository from "./repositories/user";
import modelRepository from "./repositories/model";
import exportRepository from "./repositories/export";
import instanceRepository from "./repositories/instance";
import rackRepository from "./repositories/rack";
import reportRepository from "./repositories/mock/report";
import datacenterRepository from "./repositories/mock/datacenter"
import auth from './auth';

export default {
	userRepository: userRepository,
	modelRepository: modelRepository,
	rackRepository: rackRepository,
	reportRepository: reportRepository,
	instanceRepository: instanceRepository,
	exportRepository: exportRepository,
	datacenterRepository: datacenterRepository,
	auth: auth
};