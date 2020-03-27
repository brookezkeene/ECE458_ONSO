
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
    // Get all the items for a change plan
    listItems(changeplan_id) {
        return axios.get(`${resource}/${changeplan_id}/changeplanitems`, { params: { pageSize: 2000000000 } })
            .then(response => {
                return response.data.data;
            });
    },
    // Creating a change plan
    create(item) {
        return axios.post(`${resource}/changeplan`, item).then(response => response.data).catch(error => error);
    },
    // Deleting a change plan
    delete(item) {
        return axios.delete(`${resource}/${item.id}/changeplan`).then(response => { return response.data; }).catch(error => error);
    },
    // Executing a change plan
    execute(item) {

    },
    // Creating a change plan item
    createItem(item) {
        return axios.post(`${resource}/changeplanitem`, item).then(response => response.data).catch(error => error);
    },

}