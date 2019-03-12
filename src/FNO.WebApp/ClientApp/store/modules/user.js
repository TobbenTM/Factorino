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
        commit('loadedInvitations', response.status === 204 ? [] : response.data);
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
  },
};
