<template>
  <factorio-dialog
    title="New Shipment"
    v-on:close="$emit('close')"
  >
    <div class="shipment__loader" v-if="loadingFactories">
      <app-spinner/>
    </div>
    <div class="shipment__error" v-else-if="factories.length === 0">
      <h2>Error!</h2>
      <p>No factories found</p>
    </div>
    <div class="shipment__body" v-else>
      <div class="shipment__preview" v-inlay:dark>
        <img src="/graphics/train_front.png" alt="Factorino shipment header train"/>
        <img
          v-for="(cart, index) in shipment.carts"
          :key="index"
          :src="['/graphics/train_unknown.png', '/graphics/train_cargo.png', '/graphics/train_tanker.png'][cart.cartType]"
          alt="Factorino shipment cargo/fluid wagon"
        />
        <img src="/graphics/train_rear.png" alt="Factorino shipment tail train"/>
      </div>
      <h4 class="shipment__section">Carts</h4>
      <div
        v-for="(cart, index) in shipment.carts"
        :key="index"
        class="shipment__cart"
        v-inlay:light>
        <div
          class="shipment__items"
          v-on:dragover.prevent="e => dragOver(e, cart)"
          v-on:drop.prevent="e => drop(e, cart)"
        >
          <warehouse-item
            v-for="stock in cart.inventory"
            :key="stock.name"
            :item="stock.item"
            :quantity="stock.count"
            v-on:click="removeStock(stock, cart)"
            class="shipment__item"
          />
        </div>
      </div>
      <h4 class="shipment__section">Warehouse</h4>
      <div class="shipment__warehouse" v-inlay:light>
        <div
          class="shipment__items"
        >
          <warehouse-item
            v-for="stock in availableInventory"
            :key="stock.name"
            :item="stock.item"
            :quantity="stock.count"
            draggable
            v-on:dragging="() => draggingStock = stock"
            class="shipment__item"
          />
        </div>
      </div>
      <div class="shipment__input">
        Factory:
        <multiselect
          v-model="selectedFactory"
          :options="factories"
          :custom-label="f => f.location.name"
          track-by="factoryId"
          :allow-empty="false"
          deselect-label="Must have a destination"
        />
      </div>
      <div class="shipment__input">
        Station:
        <multiselect
          v-model="selectedStation"
          :options="selectedFactory.trainStations"
          :allow-empty="false"
          deselect-label="Must have a destination"
        />
      </div>
      <div class="shipment__actions">
        <factorio-button
          small
          class="button--success"
          :disabled="reservedInventory.length === 0 || creating"
          v-on:click="create"
        >
          <icon :icon="['fas', 'dolly-flatbed']"/> Create
        </factorio-button>
      </div>
    </div>
  </factorio-dialog>
</template>

<script>
import Multiselect from 'vue-multiselect';
import { mapActions, mapState } from 'vuex';
import { CartType } from '@/enums';
import WarehouseItem from '@/components/WarehouseItem';

export default {
  components: {
    Multiselect,
    WarehouseItem,
  },
  props: {
    copyShipment: {
      type: Object,
      required: false,
    },
  },
  computed: {
    ...mapState('user', ['inventory']),
    ...mapState('factory', ['factories', 'loadingFactories']),
    reservedInventory() {
      return this.shipment.carts
        .reduce((acc, cart) => {
          const tempInventory = [...acc];

          cart.inventory.forEach(stock => {
            const existingStock = tempInventory.find(s => s.name === stock.name);
            if (existingStock) {
              existingStock.count += stock.count;
            } else {
              const copy = Object.assign({}, stock);
              tempInventory.push(copy);
            }
          });

          return tempInventory;
        }, []);
    },
    availableInventory() {
      return this.inventory.map(stock => {
        const copy = {
          count: stock.quantity,
          name: stock.itemId,
          item: stock.item,
        };
        const reserved = this.reservedInventory.find(s => s.name === copy.name);
        if (reserved) {
          copy.count -= reserved.count;
        }
        return copy;
      });
    },
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
        factoryId: null,
        destinationStation: null,
      },
      creating: false,
      draggingStock: null,
      selectedFactory: null,
      selectedStation: null,
    };
  },
  async created() {
    if (this.copyShipment) {
      Object.assign(this.shipment, this.copyShipment);
    }

    if (!this.factories || this.factories.length === 0) {
      await this.loadFactories();

      if (this.factories.length > 0) {
        this.selectedFactory = this.factories[0];
      }
    } else {
      this.selectedFactory = this.factories[0];
    }
  },
  watch: {
    selectedFactory(newFactory) {
      if (newFactory) {
        this.selectedStation = newFactory.trainStations[0];
      } else {
        this.selectedStation = null;
      }
      this.shipment.factoryId = newFactory.factoryId;
    },
    selectedStation(newStation) {
      this.shipment.destinationStation = newStation;
    },
  },
  methods: {
    ...mapActions('shipping', ['createShipment']),
    ...mapActions('factory', ['loadFactories']),
    async create() {
      this.creating = true;
      await this.createShipment(this.shipment);
      setTimeout(() => this.$emit('close'), 500);
    },
    removeStock(stock, cart) {
      cart.inventory = cart.inventory.filter(i => i !== stock);
      if (cart.inventory.length === 0) {
        cart.cartType = CartType.Unknown;
      }
    },
    isAtCapacity(cart) {
      if (cart.cartType === CartType.Unknown) {
        return false;
      }

      if (cart.cartType === CartType.Cargo) {
        const stacks = cart.inventory
          .reduce((acc, stock) => acc + (Math.ceil(stock.count/stock.item.stackSize)), 0);
        return stacks === 40;
      }

      if (cart.cartType === CartType.Fluid) {
        const fluid = cart.inventory[0].count;
        return fluid === 25000;
      }
    },
    cartCanAcceptStock(stock, cart) {
      if (cart.cartType === CartType.Unknown) {
        return true;
      }

      if (stock.item.fluid) {
        // Since fluid wagon can only fit one type of liquid,
        // we need to reject any new stock if it's not the same type
        if (cart.inventory.length > 0) {
          return cart.inventory[0].name === stock.name;
        }
        return cart.cartType === CartType.Fluid;
      }

      return cart.cartType === CartType.Cargo;
    },
    addStockToCart(stock, cart) {
      if (stock.item.fluid) {
        cart.cartType = CartType.Fluid;
        const currentStock = cart.inventory
          .reduce((acc, stock) => acc + stock.count, 0);
        const availableStock = 25000;
        this.addStockToInventory(stock, cart.inventory, availableStock - currentStock);
      } else {
        cart.cartType = CartType.Cargo;
        const currentStacks = cart.inventory
          .reduce((acc, stock) => acc + (Math.ceil(stock.count/stock.item.stackSize)), 0);
        const availableStacks = 40;
        this.addStockToInventory(stock, cart.inventory, (availableStacks - currentStacks) * stock.item.stackSize);
      }
    },
    addStockToInventory(stock, inventory, max) {
      const existingStock = inventory.find(s => s.name === stock.name);
      if (existingStock) {
        existingStock.count += Math.min(stock.count, max);
      } else {
        var copy = Object.assign({}, stock);
        copy.count = Math.min(stock.count, max);
        inventory.push(copy);
      }
    },
    dragOver(e, cart) {
      if (this.isAtCapacity(cart)) {
        e.dataTransfer.dropEffect = 'none';
        return;
      }

      if (this.cartCanAcceptStock(this.draggingStock, cart)) {
        e.dataTransfer.dropEffect = 'move';
      } else {
        e.dataTransfer.dropEffect = 'none';
      }
    },
    drop(e, cart) {
      if (this.isAtCapacity(cart) || !this.cartCanAcceptStock(this.draggingStock, cart)) {
        return;
      }

      this.addStockToCart(this.draggingStock, cart);
    },
  },
};
</script>

<style lang="scss" scoped>
.shipment {
  &__loader, &__error {
    margin: 1em;
    text-align: center;
  }

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
    height: 100%;
    overflow-y: auto;
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
    height: 100%;
    overflow-y: auto;
    position: absolute;
    box-sizing: border-box;
  }

  &__input {
    grid-column-end: span 2;
    display: flex;
    align-items: center;

    > div {
      margin-left: .5em;
    }
  }

  &__actions {
    text-align: right;
  }
}
</style>
