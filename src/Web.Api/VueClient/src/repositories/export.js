import axios from 'axios';
const resource = '/export';
export default {
    exportModel(query) {
        return axios.get(`${resource}/model/${query}`).then(response => response.data).catch(error => error);
    },
    exportInstance(query) {
        return axios.get(`${resource}/instance/${query}`).then(response => response.data).catch(error => error);
    }
}