import userRepository from "./repositories/user";
import modelRepository from "./repositories/model";
import exportRepository from "./repositories/export";
import assetRepository from "./repositories/asset";
import rackRepository from "./repositories/rack";
import reportRepository from "./repositories/report";
import datacenterRepository from "./repositories/datacenter"
import logRepository from "./repositories/log";
import changePlanRepository from "./repositories/changePlan";
import auth from './auth';
import networkRepository from './repositories/networkConnection';
import powerRepository from './repositories/powerConnection';

export default {
    userRepository: userRepository,
    modelRepository: modelRepository,
    rackRepository: rackRepository,
    reportRepository: reportRepository,
    assetRepository: assetRepository,
    exportRepository: exportRepository,
    datacenterRepository: datacenterRepository,
    logRepository: logRepository,
    auth: auth,
    networkRepository: networkRepository,
    powerRepository: powerRepository,
    changePlanRepository: changePlanRepository
};