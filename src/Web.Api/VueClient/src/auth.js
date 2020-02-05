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
                if (response.authenticated) {
                    console.log(response);

                    var token = response.token;
                    var decoded_token = jwt_decode(token);
                    console.log(decoded_token);

                    localStorage.token = decoded_token;
                    localStorage.user = response.user;
                    return true;
                } else {
                    return false;
                }
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
        //return true;
        return localStorage.user && localStorage.user.admin;
    },

}