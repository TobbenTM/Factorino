import * as signalR from '@aspnet/signalr';
import { OrderState } from '@/enums';

function findOrder(state, orderId) {
  return state.orders.find(f => f.orderId === orderId);
}

const eventHandlers = {
  OrderCreatedEvent(order) {
    order.state = OrderState.Active;
  },
  OrderPartiallyFulfilledEvent(order, event) {
    order.quantityFulfilled += event.quantityFulfilled;
    order.state = OrderState.PartiallyFulfilled;
  },
  OrderFulfilledEvent(order) {
    order.quantityFulfilled = order.quantity;
    order.state = OrderState.Fulfilled;
  },
  OrderCancelledEvent(order, event) {
    order.state = OrderState.Cancelled;
    order.cancellationReason = event.cancellationReason;
  },
};

export default {
  namespaced: true,
  state: {
    hub: null,
    orders: [],
    loadingOrders: false,
  },
  mutations: {
    hubReady(state, hub) {
      state.hub = hub;
    },
    loadingOrders(state) {
      state.loadingOrders = true;
      state.orders = [];
    },
    loadedOrders(state, orders) {
      state.orders = orders;
      state.loadingOrders = false;
    },
    createdOrder(state, order) {
      if (!findOrder(state, order.orderId)) {
        state.orders.unshift(order);
      }
    },
    cancellingOrder(state, orderId) {
      const order = findOrder(state, orderId);
      order.state = OrderState.Cancelling;
    },
    handleEvent(state, event) {
      const order = findOrder(state, event.entityId);
      if (!order) {
        console.error('Could not find order! Event:', event);
        return;
      }
      if (eventHandlers[event.eventType]) {
        eventHandlers[event.eventType](order, event);
      }
    },
  },
  actions: {
    async initHub({ commit }) {
      const hub = new signalR.HubConnectionBuilder()
        .withUrl('/ws/market')
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
    async loadOrders({ dispatch, commit, state }) {
      commit('loadingOrders');
      if (!state.hub) await dispatch('initHub');
      try {
        const orders = await state.hub.invoke('GetOrders');
        commit('loadedOrders', orders);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
    async createOrder({ commit, state }, order) {
      try {
        const createdOrder = await state.hub.invoke('CreateOrder', order);
        commit('createdOrder', createdOrder);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
    async cancelOrder({ commit, state }, orderId) {
      commit('cancellingOrder', orderId);
      try {
        await state.hub.invoke('CancelOrder', orderId);
      } catch (err) {
        commit('error', err, { root: true });
      }
    },
  },
};
