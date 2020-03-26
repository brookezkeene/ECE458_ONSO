/* eslint-disable no-unused-vars, no-console, no-debugger */
import axios from 'axios';
import Cookies from 'js-cookie';
const resource = '/login';
const loginCookie = 'auth';

const claimsType = {
    Email: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress',
    Name: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name',
    NameIdentifier: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
    GivenName: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname',
    Surname: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname',
    Role: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
}

export default {
    login(username, password) {
        return axios.post(`${resource}`, { username: username, password: password })
            .then(this.heartbeat);
    },
    heartbeat() {
        return axios.get('/users/me')
            .then((response) => {
                Cookies.remove(loginCookie);
                if (response.data !== '') {
                    Cookies.set(loginCookie, response.data);
                }
            });
    },
    logout() {
        axios.post('/logout').then(() => {
            Cookies.remove(loginCookie);
        });
    },
    loggedIn() {
        return typeof Cookies.get(loginCookie) !== 'undefined';
    },
    isAdmin() {
        let cookie = Cookies.get(loginCookie);
        if (typeof cookie !== 'undefined') {
            cookie = JSON.parse(cookie);
            const role = cookie.find(o => o.type === claimsType.Role && o.value === 'admin');
            return typeof role !== 'undefined';
        }
    },
    id() {
        let cookie = Cookies.get(loginCookie);
        if (typeof cookie != 'undefined') {
            cookie = JSON.parse(cookie);
            var id = cookie.find(o => o.type === claimsType.NameIdentifier);
            return id.value;
        }
    },
    username() {
        let cookie = Cookies.get(loginCookie);
        if (typeof cookie !== 'undefined') {
            cookie = JSON.parse(cookie);
            const username = cookie.find(o => o.type === claimsType.Name) || {};
            return username.value;
        }
    },
    permissions() {
        let cookie = Cookies.get(loginCookie);
        if (typeof cookie !== 'undefined') {
            cookie = JSON.parse(cookie);
            const datacenters = cookie.find(o => o.type === "permission:datacenter");
            return datacenters.value.split(',');
        }
    }
}