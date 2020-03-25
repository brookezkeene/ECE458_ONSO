
import axios from 'axios';
const resource = '/changePlans';

export default {
    create(item) {
        return axios.post(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    createItem(item) {
        return axios.post(`${resource}`, item).then(response => response.data).catch(error => error);
    }
}