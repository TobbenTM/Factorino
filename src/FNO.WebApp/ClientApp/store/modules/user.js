import * as signalR from '@aspnet/signalr';

const eventHandlers = {
  PlayerFactorioIdChangedEvent(player, evnt) {
    player.factorioId = evnt.factorioId;
  },
  PlayerBalanceChangedEvent(player, evnt) {
    player.credits += evnt.balanceChange;
  },
};

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
    handleEvent(state, event) {
      if (eventHandlers[event.eventType]) {
        eventHandlers[event.eventType](state.user, event);
      }
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
    async loadUser({ commit, dispatch, state }) {
      commit('loadUser');
      if (!state.hub) await dispatch('initHub');
      try {
        const player = await state.hub.invoke('GetPlayer');
        commit('loadedUser', player);
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
