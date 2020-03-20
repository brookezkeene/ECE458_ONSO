/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/assets';

export default {
    find(id) {
        return axios.get(`${resource}/${id}`)
            .then(response => {
                return response.data;
            });
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
        return axios.post(`${resource}`, item).then(response => response.data).catch(error => error);
    },
    update(item) {
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
    getDecommissionedAsset(id) {
        return axios.get(`${resource}/${id}/decommission`)
            .then(response => {
                return response.data;
            });
    },
    getDecommissionedAssets() {
        return axios.get(`${resource}/decommission`).then(response => response.data.data).catch(error => error);
    },
}