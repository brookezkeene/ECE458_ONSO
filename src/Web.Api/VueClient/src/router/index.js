import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import MainPage from '@/components/MainPage'
import ModelDetails from '@/components/ModelDetails'
import InstanceDetails from '@/components/InstanceDetails'
import RackDiagram from '@/components/RackDiagram'

Vue.use(Router)

const routes = [
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
    {
        path: '/racks/view',
        name: 'RackDiagram',
        component: RackDiagram
    }
]

export default new Router({
    mode: 'history',
    routes: routes
})