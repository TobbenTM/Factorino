import Vue from 'vue';
import Vuex from 'vuex';

import UserModule from './modules/user';
import CorporationModule from './modules/corporation';

Vue.use(Vuex);

// Mutation-types

// State
const state = {};

// Actions
const actions = {};

// Mutations
const mutations = {};

export default new Vuex.Store({
  state,
  mutations,
  actions,
  modules: {
    user: UserModule,
    corporation: CorporationModule,
  },
});
