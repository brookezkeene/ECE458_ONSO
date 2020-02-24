import axios from 'axios';
const resource = '/export';
/*eslint-disable no-unused-vars, no-console*/
export default {
    exportModel(query) {
        return axios.get(`${resource}/model`, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
    },
    exportInstance(query) {
        return axios.get(`${resource}/asset`, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
    }
}