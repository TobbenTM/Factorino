import initHub from '@/utils/signalr-hub';

const eventHandlers = {
  PlayerFactorioIdChangedEvent(state, evnt) {
    state.user.factorioId = evnt.factorioId;
  },
  PlayerBalanceChangedEvent(state, evnt) {
    state.user.credits += evnt.balanceChange;
  },
  PlayerInventoryChangedEvent(state, event) {
    event.inventoryChange.forEach(item => console.log(item));
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
        eventHandlers[event.eventType](state, event);
      }
    },
  },
  actions: {
    initHub: initHub('/ws/player'),
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
