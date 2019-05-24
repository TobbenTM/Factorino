<template>
    <factorio-panel class="market">
      <div class="market__content">
        <factorio-panel-header title="Market" class="market__header">
          <factorio-panel-action
            v-if="user"
            v-on:click="createOrder"
            :icon="['fas', 'plus']"
            class="success"
          />
        </factorio-panel-header>
        <market-filter
          class="market__filter"
          v-model="filter"
        />
        <market-orders
          class="market__orders"
          :filter="filter"
          v-inlay:dark
        />
      </div>
    </factorio-panel>
</template>

<script>
import { OrderType, OrderSearchSortColumn } from '@/enums';
import MarketFilter from '@/components/Market/MarketFilter';
import MarketOrders from '@/components/Market/MarketOrders';
import { mapState } from 'vuex';

export default {
  name: 'the-market',
  components: {
    MarketFilter,
    MarketOrders,
  },
  computed: {
    ...mapState('user', ['user']),
  },
  data() {
    return {
      filter: {
        orderType: OrderType.Buy,
        itemId: null,
        minPrice: null,
        maxPrice: null,
        sortBy: OrderSearchSortColumn.Default,
        sortDescending: false,
      },
    };
  },
  methods: {
    createOrder() {

    },
  },
};
</script>

<style lang="scss" scoped>
.market {
  margin: 15px;

  &__content {
    display: grid;
    grid-template-columns: auto 1fr;
    grid-template-rows: auto 1fr;
    grid-template-areas:
      "header header"
      "filter orders";
    height: 100%;
    padding: 0 8px;
    padding-bottom: .3em;
    box-sizing: border-box;
  }

  &__header {
    grid-area: header;
  }

  &__filter {
    grid-area: filter;
  }

  &__orders {
    grid-area: orders;
    max-height: 100%;
    overflow-y: auto;
  }
}
</style>
