/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/racks';

const splitAddress = (address) => {
    if (!validAddress(address))
        return {};
    const groups = address.match(/^([a-zA-Z])(\d+)$/);
    return { rowLetter: groups[1].toUpperCase(), rackNumber: parseInt(groups[2]) }
}

const validAddress = (address) => {
    return /^[a-zA-Z]\d+$/.test(address);
}

export default {
    list(datacenter) {
        const query = {
            datacenter: datacenter,
            pageSize: 2000000000
        }
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
    findInRange(start, end, datacenter) {
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);
        const query = {
            StartRow: startRow,
            StartCol: startCol,
            EndRow: endRow,
            EndCol: endCol,
            datacenterId: datacenter
        };

        return axios.get(`${resource}/range`, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
    },
    createInRange(start, end, datacenter) {
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);
        const query = {
            StartRow: startRow,
            StartCol: startCol,
            EndRow: endRow,
            EndCol: endCol,
            datacenterId: datacenter
        };

        return axios.post(`${resource}/range`, null, { params: query } )
    },
    deleteInRange(start, end, datacenter) {
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);
        const query = {
            StartRow: startRow,
            StartCol: startCol,
            EndRow: endRow,
            EndCol: endCol,
            datacenterId: datacenter
        };

        return axios.delete(`${resource}/range`, { params: query })
    },
    getPdus(rackid, location) {
        return axios.get(`${resource}/${rackid}/pdus`)
            .then(response => {
                return response.data;
            }).catch (error => error);
    },
    getOfflineRack(datacenterName) {
        return axios.get(`${resource}/${datacenterName}/datacenter`)
            .then(response => {
                return response.data;
            }).catch(error => error);
    }
}