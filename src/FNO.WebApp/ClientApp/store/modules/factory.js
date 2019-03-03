import axios from 'axios';

export default {
  namespaced: true,
  state: {
    factories: null,
    loadingFactories: false,
    loadedFactories: false,
  },
  mutations: {
    loadingFactories(state) {
      state.loadedFactories = false;
      state.loadingFactories = true;
      state.factories = null;
    },
    loadedFactories(state, factories) {
      state.factories = factories;
      state.loadedFactories = true;
      state.loadingFactories = false;
    },
  },
  actions: {
    async loadFactories({ commit }) {
      commit('loadingFactories');
      try {
        const response = await axios.get('/api/factory');
        commit(
          'loadedFactories',
          response.status === 204 ? null : response.data,
        );
      } catch (err) {
        if (err.response.status === 404) {
          commit('loadedFactories', null);
        } else {
          commit('error', err, { root: true });
        }
      }
    },
  },
};
