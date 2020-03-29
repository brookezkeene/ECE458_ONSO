import axios from 'axios';

const resource = '/powerConnections';

export default {
    createAll(connections) {
        return axios.post(resource, connections)
            .then(response => response.data)
            .catch(error => error);
    }
}