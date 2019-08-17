import * as signalR from '@aspnet/signalr';

function findShipment(state, shipmentId) {
  return state.shipment.find(f => f.shipmentId === shipmentId);
}

const eventHandlers = {
  // TODO
};

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
    loadingShipments(state) {
      state.loadingShipments = true;
      state.shipments = [];
    },
    loadedShipments(state, shipments) {
      state.shipments = shipments;
      state.loadingShipments = false;
    },
    handleEvent(state, event) {
      const shipment = findShipment(state, event.entityId);
      if (!shipment) {
        console.error('Could not find shipment! Event:', event);
        return;
      }
      if (eventHandlers[event.eventType]) {
        eventHandlers[event.eventType](shipment, event);
      }
    },
  },
  actions: {
    async initHub({ commit, state }) {
      // Don't want to create again if already in state
      if (state.hub) return;

      const hub = new signalR.HubConnectionBuilder()
        .withUrl('/ws/shipping')
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
    async loadShipments({ dispatch, commit, state }) {
      commit('loadingShipments');
      if (!state.hub) await dispatch('initHub');
      try {
        const orders = await state.hub.invoke('GetShipments');
        commit('loadedShipments', orders);
      } catch (err) {
        commit('error', err, { root: true });
      }
      commit('loadedShipments');
    },
  },
};
