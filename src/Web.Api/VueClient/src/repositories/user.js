/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/users';
export default {
    find(id) {
        return axios.get(`${resource}/${id}`)
            .then(response => {
                return response.data;
            });
    },
    findRole(id) {
        return axios.get(`${resource}/${id}/roles`)
            .then(response => {
                //console.log(response)
                return response.data;
            }).catch(error => error)
    },
    list() {
        return axios.get(`${resource}`, { params: { pageSize: 2000000000 } })
            .then(response => {
                return response.data.data;
            });
    },
    create(item) {
        return axios.post(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    updateUserRoles(userId, roles) {
        return axios.put(`${resource}/${userId}/roles`, roles).then(response => response.data).catch(error => error);
    },
    update(item) {
        return axios.put(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    delete(id) {
        return axios.delete(`${resource}/${id}`)
            .then(response => {
                return response.data;
            }).catch(error => error);
    }
}