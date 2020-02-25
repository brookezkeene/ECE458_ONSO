import axios from 'axios';
const resource = '/export';
/*eslint-disable no-unused-vars, no-console*/
export default {
    exportModel(query) {
        return axios.get(`${resource}/models`, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
    },
    exportInstance(query) {
        return axios.get(`${resource}/assets`, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
    }
}