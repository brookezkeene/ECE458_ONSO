
import axios from 'axios';
const resource = '/changePlans';

export default {
    list() {
        return axios.get(`${resource}`, { params: { pageSize: 2000000000 } })
            .then(response => {
                return response.data.data;
            });
    },
    create(item) {
        return axios.post(`${resource}/changeplan`, item).then(response => response.data).catch(error => error);
    },
    createItem(item) {
        return axios.post(`${resource}/changeplanitem`, item).then(response => response.data).catch(error => error);
    }
}