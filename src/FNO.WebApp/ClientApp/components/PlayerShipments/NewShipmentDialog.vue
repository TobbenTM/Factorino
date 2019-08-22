<template>
  <factorio-dialog
    title="New Shipment"
    v-on:close="$emit('close')"
  >
    <div class="shipment__body">
      <div class="shipment__preview" v-inlay:dark>
        <img src="/graphics/train_front.png"/>
        <img
          v-for="(cart, index) in shipment.carts"
          :key="index"
          :src="['/graphics/train_unknown.png', '/graphics/train_cargo.png', '/graphics/train_tanker.png'][cart.cartType]"
        />
        <img src="/graphics/train_rear.png"/>
      </div>
      <h4 class="shipment__section">Carts</h4>
      <div
        v-for="(cart, index) in shipment.carts"
        :key="index"
        class="shipment__cart"
        v-inlay:light>
        <div class="shipment__items">
          <warehouse-item
            v-for="stock in cart.inventory"
            :key="stock.warehouseInventoryId"
            :stock="stock"
            class="shipment__item"
          />
        </div>
      </div>
      <h4 class="shipment__section">Warehouse</h4>
      <div class="shipment__warehouse" v-inlay:light>
        <div class="shipment__items">
          <warehouse-item
            v-for="stock in inventory"
            :key="stock.warehouseInventoryId"
            :stock="stock"
            class="shipment__item"
          />
        </div>
      </div>
      <div class="shipment__actions">
        <factorio-button text="Create"/>
      </div>
    </div>
  </factorio-dialog>
</template>

<script>
import { mapActions, mapState } from 'vuex';
import { CartType } from '@/enums';
import WarehouseItem from '@/components/WarehouseItem';

export default {
  components: {
    WarehouseItem,
  },
  computed: {
    ...mapState('user', ['inventory']),
  },
  data() {
    return {
      shipment: {
        carts: [
          {
            cartType: CartType.Unknown,
            inventory: [],
          },
          {
            cartType: CartType.Unknown,
            inventory: [],
          },
          {
            cartType: CartType.Unknown,
            inventory: [],
          },
          {
            cartType: CartType.Unknown,
            inventory: [],
          },
          {
            cartType: CartType.Unknown,
            inventory: [],
          },
        ],
      },
      creating: false,
    };
  },
  watch: {
  },
  methods: {
    ...mapActions('shipping', ['createShipment']),
    async create() {
      this.creating = true;
      await this.createShipment(this.shipment);
      setTimeout(() => this.$emit('close'), 500);
    }
  },
};
</script>

<style lang="scss" scoped>
.shipment {
  &__body {
    margin: 8px 16px;
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    grid-template-rows: 2fr auto 2fr auto 3fr auto;
    grid-gap: 8px;
    align-items: center;
    width: 820px;
  }

  &__section {
    grid-column-start: 1;
    grid-column-end: span 5;
    margin: .1em 0 .1em .5em;
  }

  &__preview {
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    grid-column-start: 1;
    grid-column-end: span 5;
  }

  &__cart {
    padding: 1em;
    height: 100%;
    box-sizing: border-box;
  }

  &__warehouse {
    height: 100%;
    overflow-y: auto;
    box-sizing: border-box;
    grid-column-start: 1;
    grid-column-end: span 5;
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

  &__actions {
    grid-column-start: 1;
    grid-column-end: span 5;
    text-align: center;
  }
}
</style>
