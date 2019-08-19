import initHub from '@/utils/signalr-hub';
import { FactoryState } from '@/enums';

function findFactory(state, factoryId) {
  return state.factories.find(f => f.factoryId === factoryId);
}

const eventHandlers = {
  FactoryDecommissionedEvent(factory) {
    factory.state = FactoryState.Destroyed;
  },
};

export default {
  namespaced: true,
  state: {
    hub: null,
    factories: null,
    loadingFactories: false,
    loadedFactories: false,
  },
  mutations: {
    hubReady(state, hub) {
      state.hub = hub;
    },
    loadingFactories(state) {
      state.loadedFactories = false;
      state.loadingFactories = true;
      state.factories = null;
    },
    loadedFactories(state, factories) {
      state.factories = factories.map(f => ({
        ...f,
        activity: [],
      }));
      state.loadedFactories = true;
      state.loadingFactories = false;
    },
    destroyingFactory(state, factoryId) {
      const factory = findFactory(state, factoryId);
      factory.state = FactoryState.Destroying;
    },
    handleEvent(state, event) {
      const factory = findFactory(state, event.entityId);
      factory.activity.unshift(event);
      if (factory.activity.length >= 9) {
        factory.activity.pop();
      }
      if (eventHandlers[event.eventType]) {
        eventHandlers[event.eventType](factory, event);
      }
    },
  },
  actions: {
    initHub: initHub('/ws/factory'),
    async loadFactories({ dispatch, commit, state }) {
      commit('loadingFactories');
      if (!state.hub) await dispatch('initHub');
      try {
        // GetFactories will also subscribe to events for the factories
        const factories = await state.hub.invoke('GetFactories');
        commit('loadedFactories', factories);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
    async destroyFactory({ dispatch, commit, state }, factory) {
      commit('destroyingFactory', factory.factoryId);
      if (!state.hub) await dispatch('initHub');
      try {
        await state.hub.invoke('DeleteFactory', factory.factoryId);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
  },
};
