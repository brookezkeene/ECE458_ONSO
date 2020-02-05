/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/models';
export default {
    find(id) {
        return axios.get(`${resource}/${id}`)
            .then(response => {
                return response.data;
            });
    },
    list() {
        return axios.get(`${resource}`)
            .then(response => {
                return response.data;
            });
    },
    create(item) {
        return axios.post(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    update(item) {
        return axios.put(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    delete(item) {
        return axios.delete(`${resource}/${item.id}`)
            .then(response => {
                return response.data;
            }).catch(error => error);
    }
}