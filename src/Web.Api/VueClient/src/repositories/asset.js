/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/assets';

export default {
    find(id, changePlanId) {
        const endpoint = typeof changePlanId === 'undefined' || !changePlanId
            ? `${resource}/${id}`
            : `${resource}/${id}/changePlan`;

        return axios.get(endpoint)
            .then(response => response.data);
    },
    getPowerPortState(powerportid) {
        return axios.get(`${resource}/${powerportid}/power`)
            .then(response => {
                return response.data;
            })
    },
    postPowerState(id, state) {
        return axios.put(`${resource}/${id}/power`, state)
            .then(response => {
                console.log(response);
                return response.data;
            }).catch(error => {
                console.log(error);
            });
    },
    list(datacenter) {
        var query = {
            datacenter: datacenter,
            pageSize: 2000000000, 
        };
        return axios.get(`${resource}`, { params: query })
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
    create(item) {
        /*eslint-disable*/
        console.log(item);
        return axios.post(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    update(item) {
        /*eslint-disable*/
        console.log(item);
        return axios.put(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    delete(item) {
        return axios.delete(`${resource}/${item.id}`).then(response => { return response.data; }).catch(error => error);
    },
    decommission(query) {
        /*eslint-disable*/
        console.log(query);
        return axios.post(`${resource}/decommission`, null, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
    },

    getDecommissionedAssets() {
        var query = {
            pageSize: 2000000000,
        };
        return axios.get(`${resource}/decommission`, { params: query })
            .then(response => {
                return response.data.data;
            }).catch(error => error);
    },
    tablelistDecommissionedAssets(query) {
        //query contains page and pagesize
        return axios.get(`${resource}/decommission`, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
    },
}
