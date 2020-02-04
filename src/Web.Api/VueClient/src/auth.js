/* globals localStorage */

export default {
    login(username, pass) {
        if (localStorage.token) {
            return Promise.resolve(true);
        }
        return pretendRequest(username, pass).then(
            (res) => {
                if (res.authenticated) {
                    localStorage.token = res.token;
                    localStorage.user = res.user;
                    return true;
                } else {
                    return false;
                }
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
        return true;
        //return !!localStorage.token;
    },

    isAdmin() {
        //return true;
        return localStorage.user && localStorage.user.admin;
    },

}

function pretendRequest(username) {
    return Promise.resolve({
        authenticated: true,
        token: Math.random().toString(36).substring(7),
        user: {
            admin: username === 'admin'
        }
    })
}