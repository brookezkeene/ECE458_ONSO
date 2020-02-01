import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import MainPage from '@/components/MainPage'
import RackDiagram from '@/components/RackDiagram';

Vue.use(Router)

export default new Router({
    mode: 'history',
    routes: [
        {
            path: '/Main',
            name: 'MainPage',
            component: MainPage
        },
        {
            path: '/',
            name: 'Welcome',
            component: Login
        },
        {
            path: '/racks/view',
            name: 'RackDiagram',
            component: RackDiagram
        }
    ]
})