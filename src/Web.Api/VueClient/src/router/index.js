import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import ModelDetails from '@/components/ModelDetails'
import InstanceDetails from '@/components/InstanceDetails'
import RackDiagram from '@/components/RackDiagram'
import Models from '@/components/Models'
import Instances from '@/components/Instances'
import Racks from '@/components/Racks'
import ImportExport from '@/components/Bulk'
import Users from '@/components/Users'
import InstanceEdit from '@/components/InstanceEdit'
import Dashboard from '@/components/Dashboard'

import auth from '@/auth'

Vue.use(Router)

const routes = [
    {
        path: '/login',
        name: 'login',
        component: Login
    },
    {
        path: '',
        name: 'dashboard',
        component: Dashboard,
        meta: { requiresAuth: true },
        children: [
            {
                path: '',
                component: Models,
            },
            {
                path: '/models',
                name: 'model',
                component: Models,
            },
            {
                path: '/models/:id',
                name: 'model-details',
                component: ModelDetails,
                props: true,
            },
            {
                path: '/instances',
                name: 'instances',
                component: Instances,
            },
            {
                path: '/instances/:id',
                name: 'instance-details',
                component: InstanceDetails,
                props: true,
            },
            {
                path: '/instances/edit/:editedItem/:isNew',
                name: 'instance-edit',
                component: InstanceEdit,
                props: true,
                meta: { admin: true }
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
                meta: { admin: true }
            },
            {
                path: '/racks/view',
                name: 'RackDiagram',
                component: RackDiagram,
            },
            {
                path: '/users',
                name: 'users',
                component: Users,
                meta: { admin: true }
            }

            ]
    },
    
]

const router = new Router({
    mode: 'history',
    routes: routes
})

router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (!auth.loggedIn()) {
            next({
                path: '/login',
                query: { redirect: to.fullPath }
            })
        } else if (to.matched.some(record => record.meta.admin)) {
            if (!auth.isAdmin()) {
                next(false)
            } else {
                next()
            }
        } else {
            next()
        }
    } else {
        next()
    }
})

export default router;