
/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/audit-logs';
export default {
    list() {
        return axios.get(`${resource}`)
            .then(response => {
                return response.data;
            });
    },

}