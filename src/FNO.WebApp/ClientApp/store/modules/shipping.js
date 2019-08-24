import initHub from '@/utils/signalr-hub';
import { ShipmentState } from '@/enums';

function findShipment(state, shipmentId) {
  return state.shipments.find(f => f.shipmentId === shipmentId);
}

const eventHandlers = {
  ShipmentRequestedEvent(shipment) {
    shipment.state = ShipmentState.Requested;
  },
  ShipmentFulfilledEvent(shipment) {
    shipment.state = ShipmentState.Fulfilled;
  },
  ShipmentReceivedEvent(shipment) {
    shipment.state = ShipmentState.Received;
  },
  ShipmentCompletedEvent(shipment) {
    shipment.state = ShipmentState.Completed;
  },
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
    createdShipment(state, shipment) {
      shipment.state = ShipmentState.Unknown;
      if (!findShipment(state, shipment.shipmentId)) {
        state.orders.unshift(shipment);
      }
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
    initHub: initHub('/ws/shipping'),
    async loadShipments({ dispatch, commit, state }) {
      commit('loadingShipments');
      if (!state.hub) await dispatch('initHub');
      try {
        const shipments = await state.hub.invoke('GetShipments');
        console.log(shipments);
        commit('loadedShipments', shipments);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
    async createShipment({ commit, state }, shipment) {
      try {
        const createdShipment = await state.hub.invoke('CreateShipment', shipment);
        commit('createdShipment', createdShipment);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
  },
};
