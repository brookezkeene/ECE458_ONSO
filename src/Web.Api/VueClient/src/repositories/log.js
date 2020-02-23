
/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/audit-logs';
export default {
    finlistd(id) {
        return axios.get(`${resource}/${id}`)
            .then(response => {
                return response.data;
            });
    },

}