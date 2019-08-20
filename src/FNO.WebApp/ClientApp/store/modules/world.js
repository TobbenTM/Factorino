import initHub from '@/utils/signalr-hub';

const eventHandlers = {
  PlayerBalanceChangedEvent(state, evnt) {
    const player = state.highscores.find(p => p.playerId === evnt.entityId);
    if (player) {
      player.credits += evnt.balanceChange;
      state.highscores = state.highscores.sort((a, b) => a.credits > b.credits);
    }
  },
};

export default {
  namespaced: true,
  state: {
    hub: null,
    highscores: [],
    loadingHighscores: false,
    transactions: [],
    loadingTransactions: false,
  },
  mutations: {
    hubReady(state, hub) {
      state.hub = hub;
    },
    loadingHighscores(state) {
      state.loadingHighscores = true;
    },
    loadedHighscores(state, highscores) {
      state.highscores = highscores;
      state.loadingHighscores = false;
    },
    handleEvent(state, event) {
      if (eventHandlers[event.eventType]) {
        eventHandlers[event.eventType](state, event);
      }
    },
  },
  actions: {
    initHub: initHub('/ws/world'),
    async loadHighscores({ commit, dispatch, state }) {
      commit('loadingHighscores');
      if (!state.hub) await dispatch('initHub');
      try {
        const highscores = await state.hub.invoke('GetHighscores');
        commit('loadedHighscores', highscores);
      } catch (err) {
        commit('loadedHighscores', null);
      }
    },
  },
};
