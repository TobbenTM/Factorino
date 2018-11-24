import Vue from 'vue';
import Vuex from 'vuex';
import axios from 'axios';

import UserModule from './modules/user';
import CorporationModule from './modules/corporation';

Vue.use(Vuex);

// State
const rootState = {
  xsrf: null,
  error: null,
};

// Mutations
const mutations = {
  xsrfTokenFetched(state, token) {
    state.xsrf = token;
  },
  error(state, err) {
    state.error = err;
  },
};

// Actions
const actions = {
  async getXsrfToken({ commit }) {
    try {
      const response = await axios.get('/api/xsrf');
      commit('xsrfTokenFetched', response.data);
    } catch (err) {
      commit('error', err);
    }
  },
};

// Getters
const getters = {
  api: (state) => axios.create({
    headers: {
      RequestVerificationToken: state.xsrf && state.xsrf.token,
    },
  }),
};

export default new Vuex.Store({
  rootState,
  mutations,
  actions,
  getters,
  modules: {
    user: UserModule,
    corporation: CorporationModule,
  },
});
