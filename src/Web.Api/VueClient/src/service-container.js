import userRepository from "./repositories/user";
import modelRepository from "./repositories/model";
import exportRepository from "./repositories/export";
import assetRepository from "./repositories/asset";
import rackRepository from "./repositories/rack";
import reportRepository from "./repositories/mock/report";
import datacenterRepository from "./repositories/datacenter"
import logRepository from "./repositories/log";
import auth from './auth';

export default {
	userRepository: userRepository,
	modelRepository: modelRepository,
	rackRepository: rackRepository,
	reportRepository: reportRepository,
	assetRepository: assetRepository,
	exportRepository: exportRepository,
	datacenterRepository: datacenterRepository,
	logRepository: logRepository,
	auth: auth
};