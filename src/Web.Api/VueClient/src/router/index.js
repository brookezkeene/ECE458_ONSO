import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import ModelDetails from '@/components/ModelDetails'
import InstanceDetails from '@/components/InstanceDetails'
import RackDiagram from '@/components/RackDiagram'
import Models from '@/components/Models'
import Instances from '@/components/Instances'
import Racks from '@/components/Racks'
import ImportExport from '@/components/ImportFile'
import Users from '@/components/Users'

Vue.use(Router)

const routes = [
    {
        path: '/',
        name: 'Welcome',
        component: Login
    },
    {
        path: '/Models',
        name: 'model',
        component: Models
    },
    {
        path: '/Models/:id',
        name: 'model-details',
        component: ModelDetails,
        props: true
    },
    {
        path: '/Instances',
        name: 'instances',
        component: Instances
    },
    {
        path: '/Instances/:id',
        name: 'instance-details',
        component: InstanceDetails,
        props: true
    },
    {
        path: '/racks',
        name: 'racks',
        component: Racks,
    },
    {
        path: '/importexport',
        name: 'import-export',
        component: ImportExport,
    },
    {
        path: '/racks/view',
        name: 'RackDiagram',
        component: RackDiagram
    },
    {
        path: '/users',
        name: 'users',
        component: Users,
    }
]

export default new Router({
    mode: 'history',
    routes: routes
})