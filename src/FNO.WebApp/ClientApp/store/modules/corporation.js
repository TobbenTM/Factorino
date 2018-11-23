import axios from 'axios';

export default {
  namespaced: true,
  state: {
    corporation: null,
    loadingCorporation: false,
    loadedCorporation: false,
  },
  mutations: {
    loadCorporation(state) {
      state.loadedCorporation = false;
      state.loadingCorporation = true;
      state.corporation = null;
    },
    loadedCorporation(state, corporation) {
      state.corporation = corporation;
      state.loadedCorporation = true;
      state.loadingCorporation = false;
    },
  },
  actions: {
    async loadCorporation({ commit }) {
      commit('loadCorporation');
      try {
        const response = await axios.get('/api/corporation');
        commit('loadedCorporation', response.status === 204 ? null : response.data);
      } catch (err) {
        commit('loadedCorporation', null);
      }
    },
  },
};
