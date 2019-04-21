import axios from 'axios';
import * as signalR from '@aspnet/signalr';
import shipments from './player/shipments';

export default {
  namespaced: true,
  state: {
    hub: null,
    user: null,
    loadedUser: false,
    loadingUser: false,
    inventory: [],
    loadingInventory: false,
  },
  modules: {
    shipments,
  },
  mutations: {
    hubReady(state, hub) {
      state.hub = hub;
    },
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
    loadingInventory(state) {
      state.loadingInventory = true;
      state.inventory = [];
    },
    loadedInventory(state, inventory) {
      state.inventory = inventory;
      state.loadingInventory = false;
    },
  },
  actions: {
    async initHub({ commit }) {
      const hub = new signalR.HubConnectionBuilder()
        .withUrl('/ws/player')
        .build();

      // We'll also be handling all events coming through the subscription
      hub.on('ReceiveEvent', (event, eventType) => {
        commit('handleEvent', {
          ...event,
          eventType,
        });
      });

      try {
        await hub.start();
        commit('hubReady', hub);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
    async loadUser({ commit }) {
      commit('loadUser');
      try {
        const response = await axios.get('/api/player');
        commit('loadedUser', response.status === 204 ? null : response.data);
      } catch (err) {
        commit('loadedUser', null);
      }
    },
    async loadInventory({ commit, dispatch, state }) {
      commit('loadingInventory');
      if (!state.hub) await dispatch('initHub');
      try {
        const inventory = await state.hub.invoke('GetInventory');
        commit('loadedInventory', inventory);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
  },
};
