
/* eslint-disable no-unused-vars, no-console */

import axios from 'axios';
const resource = '/audit-logs';
export default {
    list(page, pageSize, search) {
        return axios.get(`${resource}`, { params: { search: search, page: page, pageSize: pageSize } })
            .then(response => {
                return response.data;
            });
    },

}