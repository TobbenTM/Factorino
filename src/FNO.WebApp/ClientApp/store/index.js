import Vue from 'vue';
import Vuex from 'vuex';
import axios from 'axios';

import UserModule from './modules/user';
import CorporationModule from './modules/corporation';
import FactoryModule from './modules/factory';

Vue.use(Vuex);

// State
const rootState = {
  xsrf: null,
  loadedXsrf: false,
  error: null,
  locations: [],
  loadedLocations: false,
  navMenuActive: false,
};

// Mutations
const mutations = {
  xsrfTokenFetched(state, token) {
    state.xsrf = token;
    state.loadedXsrf = true;
  },
  error(state, err) {
    state.error = err;
  },
  locationsLoaded(state, locations) {
    state.locations = locations;
    state.loadedLocations = true;
  },
  navMenuToggled(state) {
    state.navMenuActive = !state.navMenuActive;
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
  async loadLocations({ commit }) {
    try {
      const response = await axios.get('/api/factory/locations');
      commit('locationsLoaded', response.data);
    } catch (err) {
      commit('error', err);
    }
  },
  toggleNavMenu({ commit }) {
    commit('navMenuToggled');
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
  state: rootState,
  mutations,
  actions,
  getters,
  modules: {
    user: UserModule,
    corporation: CorporationModule,
    factory: FactoryModule,
  },
});
