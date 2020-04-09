import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import ModelDetails from '@/components/ModelDetails'
import assetDetails from '@/components/AssetDetails'
import RackDiagram from '@/components/RackDiagram'
import ModelEdit from '@/components/ModelEdit'
import Models from '@/components/Models'
import assets from '@/components/Assets'
import Racks from '@/components/Racks'
import Datacenters from '@/components/DatacenterTable'
import DatacenterEdit from '@/components/DatacenterEdit'
import ImportExport from '@/components/Bulk'
import Users from '@/components/Users'
import assetEdit from '@/components/AssetEdit'
import Reports from '@/components/Reports'
import App from '@/App'
import UsersCreate from '@/components/UserForm'
import Log from '@/components/Log'
import auth from '@/auth'
import DecommissionedAssets from '@/components/DecommissionedAssets'
import DecommissionedAssetDetails from '@/components/DecommissionedAssetDetails'
import ChangePlanner from '@/components/ChangePlanner'
import ChangePlannerDetails from '@/components/ChangePlannerDetails'
import ChangePlanEdit from '@/components/ChangePlanEdit'
import AssetLabels from '@/components/AssetLabels'
import WorkOrder from '@/components/PrintableChangePlan'
import store from '@/store/store'

Vue.use(Router)

const routes = [
    {
        path: '/login',
        name: 'login',
        component: Login
    },
    {
        path: '/',
        component: App,
        meta: { requiresAuth: true },
        children: [
            {
                path: '',
                name: 'dashboard',
                component: Models,
            },
            {
                path: '/models',
                name: 'models',
                component: Models,
            },
            {
                path: '/models/edit/:id',
                name: 'model-edit',
                component: ModelEdit,
                props: true,
                meta: { permission: 'model' }
            },
            {
                path: '/models/new',
                name: 'model-create',
                component: ModelEdit,
                props: true,
                meta: { permission: 'model' }
            },
            {
                path: '/models/:id',
                name: 'model-details',
                component: ModelDetails,
                props: true,
            },
            // Will use the same path for decommissioned, offline, and active assets
            {
                path: '/assets/:type',
                name: 'assets',
                component: assets,
                props: true
            },
            {
                path: 'assets/:type/:id',
                name: 'asset-details',
                component: assetDetails,
                props: true,
            },
            {
                path: '/assets/:type/edit/:id',
                name: 'asset-edit',
                component: assetEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: '/assets/:type/new',
                name: 'asset-new',
                component: assetEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: 'assets/:type/labels',
                name: 'asset-labels',
                component: AssetLabels,
                props: true
            },
            // will be deprecated once the schema is fixed
            {
                path: 'assets/:type',
                name: 'decommissioned',
                component: DecommissionedAssets,
            },
            {
                path: '/decommissioned-assets/:id',
                name: 'decommissioned-asset-details',
                component: DecommissionedAssetDetails,
                props: true,
            },
            {
                path: '/change-plan',
                name: 'change-plan',
                component: ChangePlanner,
                meta: { permission: 'asset' }
            },
            {
                path: '/change-plan/new',
                name: 'change-plan-new',
                component: ChangePlanEdit,
                meta: { permission: 'asset' }
            },
            {
                path: '/change-plan/edit/:id',
                name: 'change-plan-edit',
                component: ChangePlanEdit,
                meta: { permission: 'asset'}
            },
            {
                path: '/change-plan/details/:id',
                name: 'change-plan-details',
                component: ChangePlannerDetails,
                props: true,
            },
            {
                path: '/change-plan/work-order/:id',
                name: 'work-order',
                component: WorkOrder,
                props: true,
            },
            {
                path: '/racks',
                name: 'racks',
                component: Racks,
            },
            {
                path: 'sites/:type/',
                name: 'datacenters',
                component: Datacenters,
                props: true,
            },
            {
                path: 'sites/:type/edit/:id',
                name: 'datacenter-edit',
                component: DatacenterEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: 'sites/:type/new',
                name: 'datacenter-create',
                component: DatacenterEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: 'sites/:type/',
                name: 'offline-storage',
                component: Datacenters,
                props: true,
            },
            {
                path: 'sites/:type/edit/:id',
                name: 'offline-storage-edit',
                component: DatacenterEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: 'sites/:type/new',
                name: 'offline-storage-create',
                component: DatacenterEdit,
                props: true,
                meta: { permission: 'asset' }
            },
            {
                path: '/import-export',
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
                path: '/users/new',
                name: 'users-create',
                component: UsersCreate,
                meta: { permission: 'admin' }
            },
            {
                path: '/reports',
                name: 'reports',
                component: Reports,
            },
            {
                path: '/log',
                name: 'log',
                component: Log,
                meta: { permission: 'audit' }
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
        auth.heartbeat().then(() => {
                if (!auth.loggedIn()) {
                    next({
                        path: '/login',
                        query: { redirect: to.fullPath }
                    })
                } else if (to.matched.some(record => record.meta.permission)) {
                    // implementation of nav guards for multiple permissions (ev.3)
                    if (store.getters.getPermissions.includes(to.meta.permission) || auth.isAdmin()) {
                        next()
                    } else {
                        next(false)
                    }
                } else {
                    next()
                }
            })
            .catch(() => {
                // the only reason we should get here is if the user loses internet connection or the server goes down
                // display something? a red banner at the top of the page maybe?
            });
    } else {
        next()
    }
})

export default router;