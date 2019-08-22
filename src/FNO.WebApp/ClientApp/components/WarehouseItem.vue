<template>
  <div
    class="warehouse-item"
    v-inlay:dark.square
  >
    <factorio-icon
      v-on:click="$emit('selected', stock)"
      :path="stock.item.icon"
    />
    <span>{{ stock.quantity | humanizeNumber }}</span>
  </div>
</template>

<script>
export default {
  props: {
    stock: {
      type: Object,
    },
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

<style lang="scss">
.warehouse-item {
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
</style>
