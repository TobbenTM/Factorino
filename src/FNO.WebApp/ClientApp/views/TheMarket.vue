<template>
  <factorio-panel title="Market" class="market">
    <div class="market__content">
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
import { OrderType } from '@/enums';
import MarketFilter from '@/components/Market/MarketFilter';
import MarketOrders from '@/components/Market/MarketOrders';

export default{
  name: 'the-market',
  components: {
    MarketFilter,
    MarketOrders,
  },
  data() {
    return {
      filter: {
        orderType: OrderType.Buy,
        itemId: null,
        minPrice: null,
        maxPrice: null,
      },
    };
  },
};
</script>

<style lang="scss" scoped>
.market {
  margin: 15px;

  &__content {
    display: grid;
    grid-template-columns: auto 1fr;
    height: 100%;
  }
}
</style>
