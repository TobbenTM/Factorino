<template>
  <factorio-panel class="orders">
    <div class="orders__grid">
      <factorio-panel-header title="Orders">
        <factorio-panel-action
          v-on:click="createOrder"
          :icon="['fas', 'plus']"
          class="success"
        />
      </factorio-panel-header>
      <div
        class="orders__loading"
        v-if="loadingOrders"
        v-inlay:light
      >
        <icon :icon="['fas', 'spinner']" spin/> Loading orders..
      </div>
      <div
        class="orders__empty"
        v-else-if="orders.length == 0"
        v-inlay:light
      >
        No orders found!
      </div>
      <div
        class="orders__list"
        v-else
        v-inlay:light
      >
        <order-item
          v-for="order in orders"
          :key="order.orderId"
        >

        </order-item>
      </div>
    </div>
    <new-order-dialog
      v-if="creatingOrder"
      v-on:close="creatingOrder = false"
    />
  </factorio-panel>
</template>

<script>
import { mapState } from 'vuex';
import NewOrderDialog from './NewOrderDialog';

export default {
  components: {
    NewOrderDialog,
  },
  computed: {
    ...mapState('user', [ 'orders', 'loadingOrders' ]),
  },
  data() {
    return {
      creatingOrder: false,
    };
  },
  methods: {
    createOrder() {
      this.creatingOrder = true;
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';

.orders {

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
