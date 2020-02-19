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
    list() {
        return axios.get(`${resource}`)
            .then(response => {
                return response.data;
            });
    },
    findInRange(start, end) {
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);
        const query = {
            StartRow: startRow,
            StartCol: startCol,
            EndRow: endRow,
            EndCol: endCol
        };

        return axios.get(`${resource}/range`, { params: query })
            .then(response => {
                return response.data;
            }).catch(error => error);
    },
    createInRange(start, end) {
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);
        const query = {
            StartRow: startRow,
            StartCol: startCol,
            EndRow: endRow,
            EndCol: endCol
        };

        return axios.post(`${resource}/range`, null, { params: query } ).then(response => response.data).catch(error => error);
    },
    deleteInRange(start, end) {
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);
        const query = {
            StartRow: startRow,
            StartCol: startCol,
            EndRow: endRow,
            EndCol: endCol
        };

        return axios.delete(`${resource}/range`, { params: query } )
            .then(response => {
                return response.data;
            }).catch(error => error);
    }
}