<template>
  <factorio-panel class="shipments">
    <div class="shipments__grid">
      <factorio-panel-header title="Shipments">
        <factorio-panel-action
          v-on:click="createShipment"
          :icon="['fas', 'plus']"
          class="success"
        />
      </factorio-panel-header>
      <div
        class="shipments__loading"
        v-if="loadingShipments"
        v-inlay:light
      >
        <icon :icon="['fas', 'spinner']" spin/> Loading shipments..
      </div>
      <div
        class="shipments__empty"
        v-else-if="shipments.length == 0"
        v-inlay:light
      >
        No shipments found!
      </div>
      <div
        class="shipments__list"
        v-else
        v-inlay:light
      >
        <shipment-item
          v-for="shipment in shipments"
          :key="shipment.shipmentId"
        >

        </shipment-item>
      </div>
    </div>
  </factorio-panel>
</template>

<script>
import { mapState } from 'vuex';

export default {
  computed: {
    ...mapState('user/shipments', [ 'shipments', 'loadingShipments' ]),
  },
  methods: {
    createShipment() {

    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';

.shipments {

  &__grid {
    height: 100%;
    padding: 0 8px;
    display: grid;
    grid-template-columns: 1fr;
    grid-template-rows: auto 1fr;
    padding-bottom: .3em;
    box-sizing: border-box;
  }

  &__empty, &__loading {
    display: flex;
    justify-content: center;
    align-items: center;
    color: #666666;
    font-size: 2em;
    text-shadow: 2px 2px 3px rgba(0, 0, 0, 0.5);

    svg {
      margin-right: .2em;
    }
  }

  &__list {
    overflow-y: scroll;
  }
}
</style>
