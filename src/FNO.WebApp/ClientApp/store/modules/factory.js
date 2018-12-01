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
    creatingFactory(state) {
      state.creatingFactory = true;
    },
    createdFactory(state, result) {
      state.creatingFactory = false;
      console.log('Created factory, result: ', result);
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
    async createFactory({ commit, rootGetters }, factory) {
      commit('creatingFactory');
      try {
        const response = await rootGetters.api.put(
          '/api/factory',
          factory,
        );
        commit('createdFactory', response.data);
        return response.data.entityId;
      } catch (err) {
        commit('error', err, { root: true });
        throw err;
      }
    },
  },
};
