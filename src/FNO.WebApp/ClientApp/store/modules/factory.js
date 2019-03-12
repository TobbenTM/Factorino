import * as signalR from '@aspnet/signalr';
import { FactoryState } from '@/enums';

function findFactory(state, factoryId) {
  return state.factories.find(f => f.factoryId === factoryId);
}

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
      state.factories = factories;
      state.loadedFactories = true;
      state.loadingFactories = false;
    },
    destroyingFactory(state, factoryId) {
      const factory = findFactory(state, factoryId);
      factory.state = FactoryState.Destroying;
    },
  },
  actions: {
    async initHub({ commit }) {
      const hub = new signalR.HubConnectionBuilder()
        .withUrl('/ws/factory')
        .configureLogging(signalR.LogLevel.Information)
        .build();
      hub.on('ReceiveEvent', (event, eventType) => commit(`$${eventType}`, event));
      await hub.start();
      commit('hubReady', hub);
    },
    async loadFactories({ dispatch, commit, state }) {
      commit('loadingFactories');
      if (!state.hub) await dispatch('initHub');
      try {
        const factories = await state.hub.invoke('GetFactories');
        commit('loadedFactories', factories);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
    async destroyFactory({ dispatch, commit, state }, factoryId) {
      commit('destroyingFactory', factoryId);
      if (!state.hub) await dispatch('initHub');
      try {
        const result = await state.hub.invoke('DeleteFactory', factoryId);
        console.log('Destroyed factory..', result);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
  },
};
