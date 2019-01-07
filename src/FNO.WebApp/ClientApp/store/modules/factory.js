import axios from 'axios';

export default {
  namespaced: true,
  state: {
    factory: null,
    loadingFactory: false,
    loadedFactory: false,
    creatingFactory: false,
  },
  mutations: {
    loadingFactory(state) {
      state.loadedFactory = false;
      state.loadingFactory = true;
      state.factory = null;
    },
    loadedFactory(state, factory) {
      state.factory = factory;
      state.loadedFactory = true;
      state.loadingFactory = false;
    },
  },
  actions: {
    async loadFactory({ commit }) {
      commit('loadingFactory');
      try {
        const response = await axios.get('/api/factory');
        commit(
          'loadedFactory',
          response.status === 204 ? null : response.data,
        );
      } catch (err) {
        commit('loadedFactory', null);
      }
    },
  },
};
