/* eslint-disable no-unused-vars, no-console */
import jwt_decode from 'jwt-decode';
import axios from 'axios';
const resource = '/login';

export default {
    login(username, pass) {
        if (localStorage.token) {
            return Promise.resolve(true);
        }
        return axios.post(`${resource}`, { username: username, password: pass })
            .then((response) => {
                var token = response.data.authToken;
                var decoded_token = jwt_decode(token);
                localStorage.token = token;
                return true;
            }).catch((error) => {
                console.error(error.message)
            });
    },

    getToken() {
        return localStorage.token;
    },

    logout() {
        delete localStorage.token;
        return Promise.resolve();
    },

    loggedIn() {
        //return true;
        return !!localStorage.token;
    },

    isAdmin() {
        const token = localStorage.token;
        const payload = jwt_decode(token);
        return payload.rol === 'api_admin';
    },

}