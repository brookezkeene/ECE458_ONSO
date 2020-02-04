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
        // delete this when it is time
        // it is here to prevent confusion when certain features stop working
        alert("model.create(...) is not implemented");
    },
    update(item) {
        // delete this when it is time
        // it is here to prevent confusion when certain features stop working
        alert("model.update(...) is not implemented");
    },
    delete(item) {
        // delete this when it is time
        // it is here to prevent confusion when certain features stop working
        alert("model.delete(...) is not implemented");
    }
}