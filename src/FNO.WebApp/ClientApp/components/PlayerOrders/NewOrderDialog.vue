<template>
  <factorio-dialog
    title="New Marketplace Order"
    v-on:close="$emit('close')"
  >
    <div class="order__form">
      <label>Item:</label>
      <item-select v-model="selectedItem"/>
      <label>Action:</label>
      <div class="order__form__toggle">
        <factorio-button
          v-on:click="order.orderType = OrderType.Buy"
          :active="order.orderType === OrderType.Buy"
        >
          <icon :icon="['fas', 'sign-in-alt']"/> Buy
        </factorio-button>
        <factorio-button
          v-on:click="order.orderType = OrderType.Sell"
          :active="order.orderType === OrderType.Sell"
        >
          <icon :icon="['fas', 'sign-out-alt']"/> Sell
        </factorio-button>
      </div>
      <label for="new_order_quantity_input">Quantity:</label>
      <div class="order__form__field">
        <div v-inlay:light class="order__form__input">
          <input
            id="new_order_quantity_input"
            v-if="!infinite"
            type="number"
            v-model.number="order.quantity"
          >
          <span v-else class="factorino-input"><icon :icon="['fas', 'infinity']"/></span>
        </div>
        <factorio-button
          v-on:click="infinite = !infinite"
          :small="true"
          :active="infinite"
        >
          <icon :icon="['fas', 'infinity']"/>
        </factorio-button>
      </div>
      <label for="new_order_price_input" v-if="order.orderType === OrderType.Sell">Price:</label>
      <label for="new_order_price_input" v-else>Max price:</label>
      <div class="order__form__field order__form__input" v-inlay:light>
        <input
          id="new_order_price_input"
          type="number"
          v-model.number="order.price"
        >
        <span>$</span>
      </div>
      <factorio-button
        class="order__button"
        :small="true"
        :disabled="creating"
        v-on:click="create"
      >
        <template v-if="creating"><icon :icon="['fas', 'spinner']" spin/> Creating..</template>
        <template v-else><icon :icon="['fas', 'coins']"/> Create</template>
      </factorio-button>
    </div>
  </factorio-dialog>
</template>

<script>
import ItemSelect from '@/components/ItemSelect';
import { OrderType } from '@/enums';
import { mapActions } from 'vuex';

export default {
  components: {
    ItemSelect,
  },
  data() {
    return {
      OrderType,
      order: {
        itemId: null,
        orderType: OrderType.Buy,
        quantity: 0,
        price: 0,
      },
      selectedQuantity: 0,
      selectedItem: null,
      infinite: false,
      creating: false,
    };
  },
  watch: {
    selectedItem(newItem) {
      this.order.itemId = newItem.name;
    },
    infinite(isInfinite) {
      if (isInfinite) {
        this.selectedQuantity = this.order.quantity;
        this.order.quantity = -1;
      } else {
        this.order.quantity = this.selectedQuantity;
      }
    }
  },
  methods: {
    ...mapActions('market', ['createOrder']),
    async create() {
      this.creating = true;
      await this.createOrder(this.order);
      setTimeout(() => this.$emit('close'), 500);
    }
  },
};
</script>

<style lang="scss" scoped>
.order {
  &__form {
    margin: 16px;
    display: grid;
    grid-template-columns: 1fr 2fr;
    grid-gap: 8px;
    align-items: center;
    width: 420px;

    &__field {
      display: grid;
      grid-gap: 8px;
      grid-template-columns: 1fr auto;
      align-items: center;
    }

    &__input {
      padding: 0 16px;
    }

    &__toggle {
      display: grid;
      grid-template-columns: 1fr 1fr;
      align-items: center;
    }
  }

  &__button {
    margin: 16px 12px 0 12px;
    grid-column: 1 / span 2;
  }
}
</style>
