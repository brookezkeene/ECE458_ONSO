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
        username: '',
    },

    /*Defines Computed Properties for our Store
     Documentation: https://vuex.vuejs.org/guide/getters.html
     */
    getters: {

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
        /*eslint-disable*/
            console.log(username);
        }
    },

    /*Used to Commit Mutations
     Documentation: https://vuex.vuejs.org/guide/actions.html 
     AJAX(axios?) --> Actions
     */
    actions: {
        loadUsername({ commit }) {
            commit('SAVE_USER', auth.username());
        }
    },
})
