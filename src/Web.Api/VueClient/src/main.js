import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify';
import VuetifyDialog from "vuetify-dialog";
import router from  './router'
import serviceContainer from './service-container';

Vue.config.productionTip = false;
Vue.use(vuetify);
Vue.use(VuetifyDialog);
Vue.config.productionTip = false

new Vue({
  el: '#app',
  router,
  template: '<App/>',
  components: {App},
  vuetify,
  provide: serviceContainer,
  render: h => h(App),
}).$mount('#app')
 