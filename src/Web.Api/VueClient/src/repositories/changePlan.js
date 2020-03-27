
import axios from 'axios';
const resource = '/changePlans';

export default {
    list(user_id) {
        /*eslint-disable*/
        console.log(user_id);
        return axios.get(`${resource}/${user_id}/changeplans`, { params: { pageSize: 2000000000 } })
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