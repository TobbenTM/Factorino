import axios from 'axios';

export default {
  namespaced: true,
  state: {
    hub: null,
    shipments: [],
    loadingShipments: false,
  },
  mutations: {
    hubReady(state, hub) {
      state.hub = hub;
    },
    loadShipments(state) {
      state.loadingShipments = true;
      state.shipments = [];
    },
    loadedShipments(state, shipments) {
      state.shipments = shipments;
      state.loadingShipments = false;
    },
  },
  actions: {
    async loadShipments({ commit }) {
      commit('loadShipments');
      try {
        const response = await axios.get('/api/player/shipments');
        commit('loadedShipments', response.status === 204 ? [] : response.data);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
  },
};
