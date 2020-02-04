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
//import Auth from '@/auth'

Vue.use(Router)

const routes = [
    {
        path: '/',
        name: 'Welcome',
        component: Login,
        /*beforeEnter(to, from, next) {
            if (Auth.loggedIn()) {
                next({ path: '/models' });
            }
            else next();
        }*/
    },
    {
        path: '/Models',
        name: 'model',
        component: Models,
        /*beforeEnter(to, from, next) {
            if (Auth.loggedIn()) {
                next();
            }
            else next({ path: '/' });
        }*/
    },
    {
        path: '/Models/:id',
        name: 'model-details',
        component: ModelDetails,
        props: true,
        /*beforeEnter(to, from, next) {
            if (Auth.loggedIn()) {
                next();
            } 
            else next({ path: '/' });
        }*/

    },
    {
        path: '/Instances',
        name: 'instances',
        component: Instances,
        /*beforeEnter(to, from, next) {
            if (Auth.loggedIn()) {
                next();
            }
            else next({ path: '/' });
        }*/
    },
    {
        path: '/Instances/:id',
        name: 'instance-details',
        component: InstanceDetails,
        props: true,
        /*beforeEnter(to, from, next) {
            if (Auth.loggedIn()) {
                next();
            }
            else next({ path: '/' });
        }*/
    },
    {
        path: '/racks',
        name: 'racks',
        component: Racks,
        /*beforeEnter(to, from, next) {
            if (Auth.loggedIn()) {
                next();
            }
            else next({ path: '/' });
        }*/
    },
    {
        path: '/importexport',
        name: 'import-export',
        component: ImportExport,
        /*beforeEnter(to, from, next) {
            if (Auth.loggedIn()) {
                next();
            }
            else next({ path: '/' });
        }*/
    },
    {
        path: '/racks/view',
        name: 'RackDiagram',
        component: RackDiagram,
        /*beforeEnter(to, from, next) {
            if (Auth.loggedIn()) {
                next();
            }
            else next({ path: '/' });
        }*/
    },
    {
        path: '/users',
        name: 'users',
        component: Users,
        /*beforeEnter(to, from, next) {
            if (Auth.loggedIn()) {
                next();
            }
            else next({ path: '/' });
        }*/
    }
]

/*this.Router.beforeEach((to, from, next) => {
    if (!Auth.loggedIn()) next('/')
    else next();
})*/

export default new Router({
    mode: 'history',
    routes: routes
})
