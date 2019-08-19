import * as signalR from '@aspnet/signalr';

export default function initHub(url) {
  return async function hubBuilder({ commit, state }) {
    // Don't want to create again if already in state
    if (state.hub) return;

    const hub = new signalR.HubConnectionBuilder()
      .withUrl(url)
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
  };
}
