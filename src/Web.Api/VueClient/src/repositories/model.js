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
        return axios.get(`${resource}`, { params: { pageSize: 2000000000 } })
            .then(response => {
                return response.data.data;
            });
    },
    tablelist(query) {
        //query contains page and pagesize
        return axios.get(`${resource}`, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
    },
    tablelist(query) {
        //query contains page and pagesize
        return axios.get(`${resource}`, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
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
    },

}