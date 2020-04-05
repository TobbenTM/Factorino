<template>
  <div class="orders">
    <div v-if="loadingOrders" class="orders__loader">
      <app-spinner text="Loading Market.."/>
    </div>
    <div
      class="orders__empty"
      v-else-if="orders.length == 0"
    >
      No orders found!
    </div>
    <div
      v-else
      style="max-height: 100%; overflow-y: auto;"
    >
      <div class="orders__list">
        <table>
          <thead>
            <tr>
              <th scope="col">Item</th>
              <th scope="col">Order</th>
              <th scope="col">State</th>
              <th scope="col">Quantity</th>
              <th scope="col">Price</th>
              <th scope="col">Owner</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="order in orders"
              :key="order.orderId"
            >
              <td v-if="order.item">
                <factorio-icon :path="order.item.icon"/> {{ order.item.name | separate | capitalize }}
              </td>
              <td v-else>
                ??
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
              <td v-if="order.owner">{{ order.owner.name }}</td>
              <td v-else>??</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';
import {
  OrderType,
  OrderState,
  OrderCancellationReason,
} from '@/enums';
import debounce from '@/utils/debounce';

export default {
  props: {
    filter: {
      type: Object,
      required: true,
    },
  },
  computed: {
    ...mapState('market', ['orderResults', 'loadingOrders']),
    orders() {
      return this.orderResults ? this.orderResults.results : [];
    },
  },
  data() {
    return {
      OrderType,
      OrderState,
      OrderCancellationReason,
      pageIndex: 1,
    };
  },
  created() {
    this.search();
  },
  watch: {
    filter: {
      deep: true,
      handler: function(newFilter) {
        this.debounceSearch(newFilter);
      },
    },
  },
  methods: {
    ...mapActions('market', ['loadOrders']),
    search(filter) {
      this.loadOrders({ filter: filter || this.filter, pageIndex: this.pageIndex });
    },
    debounceSearch(filter) {
      // TODO: Debounce
      // debounce(() => this.search(filter), 500);
      this.search(filter);
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';
@import '@/css/variables.scss';

.orders {

  &__empty, &__loader {
    height: 100%;
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
