import initHub from '@/utils/signalr-hub';

const eventHandlers = {
  PlayerBalanceChangedEvent(state, evnt) {
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
    async loadingHighscores({ commit, dispatch, state }) {
      commit('loadHighscores');
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
