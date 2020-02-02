import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import MainPage from '@/components/MainPage'
import ModelDetails from '@/components/ModelDetails'
import InstanceDetails from '@/components/InstanceDetails'

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
            path: '/ModelDetails/:id',
            name: 'model-details',
            component: ModelDetails,
            props: true
        },
        {
            path: '/InstanceDetails/:id',
            name: 'instance-details',
            component: InstanceDetails,
            props: true
        },
    ]
})