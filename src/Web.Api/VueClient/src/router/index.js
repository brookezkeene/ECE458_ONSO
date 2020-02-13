import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import ModelDetails from '@/components/ModelDetails'
import InstanceDetails from '@/components/InstanceDetails'
import RackDiagram from '@/components/RackDiagram'
import ModelEdit from '@/components/ModelEdit'
import Models from '@/components/Models'
import Instances from '@/components/Instances'
import Racks from '@/components/Racks'
import ImportExport from '@/components/Bulk'
import Users from '@/components/Users'
import InstanceEdit from '@/components/InstanceEdit'
import Reports from '@/components/Reports'
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
                path: '/models/edit/:id',
                name: 'model-edit',
                component: ModelEdit,
                props: true,
                meta: { admin: true }
            },
            {
                path: '/models/new',
                name: 'model-create',
                component: ModelEdit,
                props: true,
                meta: { admin: true }
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
                path: '/instances/edit/:id',
                name: 'instance-edit',
                component: InstanceEdit,
                props: true,
                meta: { admin: true }
            },
            {
                path: '/instances/new',
                name: 'instance-new',
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
            },
            {
                path: '/reports',
                name: 'reports',
                component: Reports,
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