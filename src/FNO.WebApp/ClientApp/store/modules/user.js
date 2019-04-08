import axios from 'axios';

export default {
  namespaced: true,
  state: {
    user: null,
    loadedUser: false,
    loadingUser: false,

    invitations: [],
    loadingInvitations: false,

    inventory: null,
    loadingInventory: false,

    orders: [],
    loadingOrders: false,

    shipments: [],
    loadingShipments: false,
  },
  mutations: {
    loadUser(state) {
      state.loadedUser = false;
      state.loadingUser = true;
      state.user = null;
    },
    loadedUser(state, user) {
      state.user = user;
      state.loadedUser = true;
      state.loadingUser = false;
    },
    loadInvitations(state) {
      state.loadingInvitations = true;
      state.invitations = [];
    },
    loadedInvitations(state, invitations) {
      state.invitations = invitations;
      state.loadingInvitations = false;
    },
    loadInventory(state) {
      state.loadingInventory = true;
      state.inventory = [];
    },
    loadedInventory(state, inventory) {
      state.inventory = inventory;
      state.loadingInventory = false;
    },
    loadOrders(state) {
      state.loadingOrders = true;
      state.orders = [];
    },
    loadedOrders(state, orders) {
      state.orders = orders;
      state.loadingOrders = false;
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
    async loadUser({ commit }) {
      commit('loadUser');
      try {
        const response = await axios.get('/api/player');
        commit('loadedUser', response.status === 204 ? null : response.data);
      } catch (err) {
        commit('loadedUser', null);
      }
    },
    async loadInvitations({ commit }) {
      commit('loadInvitations');
      try {
        const response = await axios.get('/api/player/invitations');
        commit(
          'loadedInvitations',
          response.status === 204 ? [] : response.data,
        );
      } catch (err) {
        commit('loadedInvitations', null);
      }
    },
    async loadInventory({ commit }) {
      commit('loadInventory');
      try {
        const response = await axios.get('/api/player/inventory');
        commit('loadedInventory', response.status === 204 ? [] : response.data);
      } catch (err) {
        commit('loadedInventory', null);
      }
    },
    async loadOrders({ commit }) {
      commit('loadOrders');
      try {
        const response = await axios.get('/api/player/orders');
        commit('loadedOrders', response.status === 204 ? [] : response.data);
      } catch (err) {
        commit('loadedOrders', null);
      }
    },
    async loadShipments({ commit }) {
      commit('loadShipments');
      try {
        const response = await axios.get('/api/player/shipments');
        commit('loadedShipments', response.status === 204 ? [] : response.data);
      } catch (err) {
        commit('loadedShipments', null);
      }
    },
  },
};
