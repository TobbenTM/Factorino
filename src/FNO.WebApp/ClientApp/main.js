import Vue from 'vue';
import axios from 'axios';
import { sync } from 'vuex-router-sync';
import * as signalR from '@aspnet/signalr';
import App from './App.vue';
import router from './router';
import store from './store';
import FontAwesomeIcon from './icons';
import AppSpinner from './components/AppSpinner';
import FactorioButton from './components/FactorioButton';
import FactorioPanelHeader from './components/FactorioPanelHeader';
import FactorioPanelAction from './components/FactorioPanelAction';
import FactorioPanel from './components/FactorioPanel';
import FactorioDialog from './components/FactorioDialog';
import FactorioIcon from './components/FactorioIcon';
import Inlay from './directives/inlay';
import Numeral from './filters/numeral';
import { separate, capitalize } from './filters/string';

Vue.config.devtools = true;

// Registration of global components
Vue.component('icon', FontAwesomeIcon);
Vue.component('app-spinner', AppSpinner);
Vue.component('factorio-button', FactorioButton);
Vue.component('factorio-panel-header', FactorioPanelHeader);
Vue.component('factorio-panel-action', FactorioPanelAction);
Vue.component('factorio-panel', FactorioPanel);
Vue.component('factorio-dialog', FactorioDialog);
Vue.component('factorio-icon', FactorioIcon);

// Registration of global directives
Vue.directive('inlay', Inlay);

// Registration of global filters
Vue.filter('formatNumeral', Numeral);
Vue.filter('separate', separate);
Vue.filter('capitalize', capitalize);

Vue.prototype.$http = axios;
Vue.prototype.$signalR = signalR;

sync(store, router);

const app = new Vue({
  store,
  router,
  ...App,
});

export { app, router, store };
