import axios from 'axios';

export default {
  namespaced: true,
  state: {
    corporation: null,
    loadingCorporation: false,
    loadedCorporation: false,
    creatingCorporation: false,
  },
  mutations: {
    loadingCorporation(state) {
      state.loadedCorporation = false;
      state.loadingCorporation = true;
      state.corporation = null;
    },
    loadedCorporation(state, corporation) {
      state.corporation = corporation;
      state.loadedCorporation = true;
      state.loadingCorporation = false;
    },
    creatingCorporation(state) {
      state.creatingCorporation = true;
    },
    createdCorporation(state, result) {
      state.creatingCorporation = false;
      console.log('Created corporation, result: ', result);
    },
  },
  actions: {
    async loadCorporation({ commit }) {
      commit('loadingCorporation');
      try {
        const response = await axios.get('/api/corporation');
        commit(
          'loadedCorporation',
          response.status === 204 ? null : response.data,
        );
      } catch (err) {
        commit('loadedCorporation', null);
      }
    },
    async createCorporation({ commit, rootGetters }, corporation) {
      commit('creatingCorporation');
      try {
        const response = await rootGetters.api.put(
          '/api/corporation',
          corporation,
        );
        commit('createdCorporation', response.data);
        return response.data.entityId;
      } catch (err) {
        commit('error', err, { root: true });
        throw err;
      }
    },
    async leaveCorporation({ commit, rootGetters }) {
      commit('leavingCorporation');
      try {
        const response = await rootGetters.api.patch('/api/corporation/leave');
        commit('leftCorporation', response.data);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
  },
};
