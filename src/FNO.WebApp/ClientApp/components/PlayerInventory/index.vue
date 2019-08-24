<template>
  <factorio-panel class="warehouse">
    <div class="warehouse__grid">
      <factorio-panel-header title="Warehouse"/>
      <div
        class="warehouse__loading"
        v-if="loadingInventory"
        v-inlay:light
      >
        <icon :icon="['fas', 'spinner']" spin/> Loading warehouse..
      </div>
      <div
        style="max-height: 100%; overflow-y: auto;"
        v-inlay:light
        v-else
      >
        <div class="warehouse__items">
          <!-- A div per inventory item -->
          <warehouse-item
            v-for="stock in inventory"
            :key="stock.warehouseInventoryId"
            :item="stock.item"
            :quantity="stock.quantity"
            class="warehouse__items__item"
            v-inlay:dark.square
          />
        </div>
      </div>
      <div class="warehouse__stats">
        <span>Total items: {{ totalItems | formatNumeral('0.0a') }}</span>
        <span>Net worth: {{ netWorth | formatNumeral('0.0a $') }}</span>
        <span>Slots used (stacks): {{ totalStacks | formatNumeral('0.0a') }} / <icon :icon="['fas', 'infinity']"/></span>
      </div>
    </div>
  </factorio-panel>
</template>

<script>
import { mapState, mapActions } from 'vuex';
import WarehouseItem from '@/components/WarehouseItem';

export default {
  components: {
    WarehouseItem,
  },
  computed: {
    ...mapState('user', ['inventory', 'loadingInventory']),
    totalItems() {
      return this.inventory.reduce((acc, cur) => acc + cur.quantity, 0);
    },
    netWorth() {
      // TODO: Change this when we can get the item value
      return this.inventory.reduce((acc, cur) => acc + cur.quantity, 0);
    },
    totalStacks() {
      return this.inventory.reduce((acc, cur) => cur.item.stackSize === 0 ? 1 : acc + Math.ceil(cur.quantity / cur.item.stackSize), 0);
    },
  },
  mounted() {
    this.loadInventory();
  },
  methods: {
    ...mapActions('user', ['loadInventory']),
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';

.warehouse {

  &__grid {
    height: 100%;
    padding: 0 8px;
    display: grid;
    grid-template-columns: 1fr;
    grid-template-rows: auto 1fr auto;
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

  &__items {
    display: flex;
    align-content: flex-start;
    flex-wrap: wrap;
    padding: .5em;
    width: 100%;
    max-height: 100%;
    overflow-y: auto;
    position: absolute;
    box-sizing: border-box;
  }

  &__stats {
    display: flex;
    justify-content: space-between;
    padding-top: .8em;
  }
}
</style>
