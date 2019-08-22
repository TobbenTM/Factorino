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
          <div
            v-for="stock in inventory"
            :key="stock.warehouseInventoryId"
            class="warehouse__items__item"
            v-inlay:dark.square
          >
            <factorio-icon
              v-on:click="$emit('selected', stock)"
              :path="stock.item.icon"
            />
            <span>{{ stock.quantity | humanizeNumber }}</span>
          </div>
        </div>
      </div>
      <div class="warehouse__stats">
        <span>Total items: {{ totalItems | humanizeNumber }}</span>
        <span>Net worth: {{ netWorth }} $</span>
        <span>Slots used (stacks): {{ totalStacks }} / <icon :icon="['fas', 'infinity']"/></span>
      </div>
    </div>
  </factorio-panel>
</template>

<script>
import { mapState, mapActions } from 'vuex';

export default {
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
      return this.inventory.reduce((acc, cur) => acc + Math.ceil(cur.quantity / cur.item.stackSize), 0);
    },
  },
  mounted() {
    this.loadInventory();
  },
  methods: {
    ...mapActions('user', ['loadInventory']),
  },
  filters: {
    humanizeNumber(number) {
      if (number >= 1000000) {
        return `${Math.round(number / 1000000)}M`;
      }
      if (number >= 1000) {
        return `${Math.round(number / 1000)}k`;
      }
      return number;
    },
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

    &__item {
      display: inline;
      font-size: 2em;
      margin-right: .2em;
      max-width: 48px;
      min-width: 48px;
      max-height: 48px;
      min-height: 48px;
      position: relative;

      > span {
        position: absolute;
        font-size: .4em;
        text-shadow: 0 0 .5em #000;
        bottom: 0;
        right: 0;
        user-select: none;
        margin-right: .3em;
        margin-bottom: .2em;
      }

      > img {
        box-sizing: border-box;
        cursor: pointer;

        &:hover {
          background: #ff9f1b;
        }
      }
    }
  }

  &__stats {
    display: flex;
    justify-content: space-between;
    padding-top: .8em;
  }
}
</style>
