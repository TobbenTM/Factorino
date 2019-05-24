<template>
  <div class="filter">
    <div class="filter__input">
      <label>OrderType</label>
      <div class="filter__input__toggle">
        <factorio-button
          v-on:click="selectedOrderType = OrderType.Buy"
          :active="selectedOrderType === OrderType.Buy"
        >
          <icon :icon="['fas', 'sign-in-alt']"/> Buy
        </factorio-button>
        <factorio-button
          v-on:click="selectedOrderType = OrderType.Sell"
          :active="selectedOrderType === OrderType.Sell"
        >
          <icon :icon="['fas', 'sign-out-alt']"/> Sell
        </factorio-button>
      </div>
    </div>
    <div class="filter__input">
      <label>Item</label>
      <item-select v-model="selectedItem"/>
    </div>
    <div class="filter__input">
      <label>Minimum price</label>
      <div class="filter__input__field" v-inlay:light>
        <input type="number" placeholder="Min price">
      </div>
    </div>
    <div class="filter__input">
      <label>Maximum price</label>
      <div class="filter__input__field" v-inlay:light>
        <input type="number" placeholder="Max price">
      </div>
    </div>
  </div>
</template>

<script>
import ItemSelect from '@/components/ItemSelect';
import { OrderType } from '@/enums';

export default {
  props: {
    value: {
      type: Object,
      required: true,
    },
  },
  components: {
    ItemSelect,
  },
  data() {
    return {
      OrderType,
      selectedOrderType: null,
      selectedItem: null,
    };
  },
  created() {
    this.selectedOrderType = this.value.orderType;
  }
};
</script>

<style lang="scss" scoped>
@import '@/css/variables.scss';

.filter {
  display: flex;
  flex-direction: column;
  padding: 15px 0 15px 15px;
  width: 320px;

  &__input {
    padding: 8px;
    margin-bottom: 2px;
    border-top: 3px solid $emboss_dark;
    border-left: 3px solid $emboss_dark;
    border-bottom: 2px solid $emboss_light;

    label {
      display: block;
      font-weight: 300;
      margin-bottom: 4px;
    }

    &__field {
      display: grid;
      grid-gap: 8px;
      grid-template-columns: 1fr auto;
      align-items: center;
      padding: 0 8px;
    }

    &__toggle {
      display: grid;
      grid-template-columns: 1fr 1fr;
      align-items: center;
    }
  }
}
</style>
