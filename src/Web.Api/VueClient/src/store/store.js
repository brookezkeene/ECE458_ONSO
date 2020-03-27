/* eslint-disable no-unused-vars, no-console */


import Vue from 'vue'
import Vuex from 'vuex'
import auth from "../auth"

Vue.use(Vuex)

export default new Vuex.Store({
    /*Stores Overall State of the App
     Documentation: https://vuex.vuejs.org/guide/state.html
     Props --> State
     */
    state: {
        clicked: false,         // for clicking network neighborhood
        itemId: '',             // for detail and edit views
        page: '',               // to keep track of current page
        dialogVisible: false,   // to show or hide a dialog
        dialogType: null,       // specify dialog type
        updateData: false,      // for updating tables
        username: '',           // for showing who is signed in and saving who decommissioned
        myPermissions: [],      // for tracking user permissions
        changePlan: false,      // for showing changeplan snackbar
        changePlanName: '',     // for showing name of changeplan being edited in snackbar
        userId: auth.id(),      // for querying the backend for the changeplans belonging to a particular user
    },

    /*Defines Computed Properties for our Store
     Documentation: https://vuex.vuejs.org/guide/getters.html
     */
    getters: {
        hasModelPermission: state => {
            return state.myPermissions.includes("model")
        },
        hasAssetPermission: state => {
            return state.myPermissions.includes("asset")
        },
        hasPowerPermission: state => {
            return state.myPermissions.includes("power")
        },
        hasAuditPermission: state => {
            return state.myPermissions.includes("audit")
        },
        isAdmin: state => {
            return state.myPermissions.includes("admin")
        },
        isChangePlan: state => {
            return state.changePlan
        },
        changePlanName: state => {
            return state.changePlanName
        },
        userId: state => {
            console.log(state.userId);
            return state.userId
        }
    },

    /*Defines functions that Change the App State
     Documentation: https://vuex.vuejs.org/guide/mutations.html
     Events --> Mutations
     */
    mutations: {
        openDialog(state, componentName) {
            state.dialogVisible = true;
            state.dialogType = componentName;
        },
        closeDialog(state) {
            state.dialogVisible = false;
        },
        setId(state, id) {
            state.itemId = id;
        },
        changePage(state, name) {
            state.page, name;
        },
        SAVE_USER(state, username) {
            state.username = username;
        },
        SAVE_ROLES(state, roles) {
            state.myPermissions = roles;
        },
        SAVE_USER_ID(state, id) {
            state.userId = id;
        },
        START_CHANGE_PLAN(state, name) {
            state.changePlanName = name;
            state.changePlan = true;
        },
        END_CHANGE_PLAN(state) {
            state.changePlan = false;
        }
    },

    /*Used to Commit Mutations
     Documentation: https://vuex.vuejs.org/guide/actions.html 
     AJAX(axios?) --> Actions
     */
    actions: {
        loadUsername({ commit }) {
            commit('SAVE_USER', auth.username());
        },
        loadPermissions({ commit }) {
            Vue.axios.get(`/users/${auth.id()}/roles`).then(response => {
                commit('SAVE_ROLES', response.data);
            }).catch(error => {
                throw new Error(`API ${error}`);
            });
        },
        // Provides the user id from the cookie to send for a changeplan
        loadUserId({ commit }) {
            commit('SAVE_USER_ID', auth.id());
            console.log(auth.id());
        },
        startChangePlan({ commit }, name) {
            commit('START_CHANGE_PLAN', name);
        },
        endChangePlan({ commit }) {
            commit('END_CHANGE_PLAN');
        }
    },
})
