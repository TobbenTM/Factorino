import Vue from 'vue';
import axios from 'axios';
import { sync } from 'vuex-router-sync';
import VueLadda from 'vue-ladda';
import * as signalR from '@aspnet/signalr';
import App from 'vm/App.vue';
import router from './router';
import store from './store';
import FontAwesomeIcon from './icons';

// Registration of global components
Vue.component('icon', FontAwesomeIcon);
Vue.component('vue-ladda', VueLadda);

Vue.prototype.$http = axios;
Vue.prototype.$signalR = signalR;

sync(store, router);

const app = new Vue({
  store,
  router,
  ...App,
});

export { app, router, store };
