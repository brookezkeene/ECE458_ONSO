import axios from 'axios';

const resource = '/networkConnections';

export default {
    createAll(connections) {
        return axios.post(resource, connections)
            .then(response => response.data)
            .catch(error => error);
    }
}