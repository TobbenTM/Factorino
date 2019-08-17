<template>
  <factorio-panel class="orders">
    <div class="orders__grid">
      <factorio-panel-header title="Orders">
        <factorio-panel-action
          v-on:click="createOrder"
          :icon="['fas', 'plus']"
          class="success"
        />
        <factorio-panel-action
          v-on:click="loadOrders"
          :icon="['fas', 'sync']"
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
        v-else
        v-inlay:light
        style="max-height: 100%;"
      >
        <div class="orders__list">
          <table>
            <thead>
              <tr>
                <th>Item</th>
                <th>Order</th>
                <th>State</th>
                <th>Quantity</th>
                <th>Price</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="order in orders"
                :key="order.orderId"
              >
                <td>
                  <factorio-icon :path="order.item.icon"/> {{ order.item.name }}
                </td>
                <td v-if="order.orderType === OrderType.Buy">
                  <icon :icon="['fas', 'sign-in-alt']"/> Buy
                </td>
                <td v-else>
                  <icon :icon="['fas', 'sign-out-alt']"/> Sell
                </td>
                <td>
                  <template v-if="order.state === OrderState.Created"><icon :icon="['fas', 'spinner']" spin/> Creating..</template>
                  <template v-else-if="order.state === OrderState.Active"><icon :icon="['fas', 'check']"/> Active</template>
                  <template v-else-if="order.state === OrderState.PartiallyFulfilled"><icon :icon="['fas', 'check']"/> Active</template>
                  <template v-else-if="order.state === OrderState.Fulfilled"><icon :icon="['fas', 'check']"/> Fulfilled</template>
                  <template v-else-if="order.state === OrderState.Cancelled"><icon :icon="['fas', 'ban']"/> Cancelled</template>
                  <template v-else-if="order.state === OrderState.Cancelling"><icon :icon="['fas', 'spinner']" spin/> Cancelling..</template>
                  <template v-else><icon :icon="['fas', 'question-circle']"/> Unknown</template>
                </td>
                <td>{{ order.quantityFulfilled }} / <icon v-if="order.quantity === -1" :icon="['fas', 'infinity']"/><span v-else>{{ order.quantity }}</span></td>
                <td>{{ order.price }} $</td>
                <td>
                  <factorio-button
                    v-if="order.state !== OrderState.Cancelled"
                    :disabled="order.state === OrderState.Cancelling"
                    :small="true"
                    v-on:click="cancelOrder(order.orderId)"
                  >
                    <template v-if="order.state === OrderState.Cancelling"><icon :icon="['fas', 'spinner']" spin/> Cancelling..</template>
                    <template v-else><icon :icon="['fas', 'ban']"/> Cancel</template>
                  </factorio-button>
                  <template v-else>
                    <template v-if="order.cancellationReason === OrderCancellationReason.NoFunds">
                      <icon :icon="['fas', 'faExclamation-triangle']"/> No funds!
                    </template>
                    <template v-if="order.cancellationReason === OrderCancellationReason.NoResources">
                      <icon :icon="['fas', 'faExclamation-triangle']"/> No resources!
                    </template>
                  </template>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <new-order-dialog
      v-if="creatingOrder"
      v-on:close="creatingOrder = false"
    />
  </factorio-panel>
</template>

<script>
import { mapState, mapActions } from 'vuex';
import {
  OrderType,
  OrderState,
  OrderCancellationReason,
} from '@/enums';
import NewOrderDialog from './NewOrderDialog';

export default {
  components: {
    NewOrderDialog,
  },
  computed: {
    ...mapState('market', [ 'orders', 'loadingOrders' ]),
  },
  data() {
    return {
      OrderType,
      OrderState,
      OrderCancellationReason,
      creatingOrder: false,
    };
  },
  created() {
    this.loadOrders();
  },
  methods: {
    ...mapActions('market', ['loadOrders', 'cancelOrder']),
    createOrder() {
      this.creatingOrder = true;
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';
@import '@/css/variables.scss';

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
    max-height: 100%;
    overflow-y: auto;

    table {
      width: 100%;
      text-align: center;
      border-collapse: separate;
      border-spacing: 0;

      thead {
        color: grey;
        background: rgba(0, 0, 0, 0.3);
      }

      tbody > tr {
      }

      td {
        padding: .2em 0;
        border-top: 1px solid $emboss_light;
        border-bottom: 2px solid $emboss_dark;

        .button {
          padding: 4px 8px;
        }
      }

      td:first-child, th:first-child {
        text-align: left;
        padding-left: .4em;
      }

      td:last-child, th:last-child {
        padding-right: .4em;
      }
    }
  }
}
</style>
