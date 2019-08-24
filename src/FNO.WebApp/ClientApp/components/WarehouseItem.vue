<template>
  <div
    class="warehouse-item"
    v-inlay:dark.square
    :draggable="draggable"
    v-on:dragstart="dragStart"
    v-on:click="$emit('click')"
  >
    <factorio-icon
      v-on:click="$emit('selected')"
      :path="item.icon"
      :name="item.name"
    />
    <span>{{ quantity | formatNumeral('0a') }}</span>
  </div>
</template>

<script>
export default {
  props: {
    item: {
      type: Object,
      required: true,
    },
    quantity: {
      type: Number,
      required: false,
    },
    draggable: {
      type: Boolean,
      required: false,
      default: false,
    },
  },
  methods: {
    dragStart(e) {
      if (!this.draggable) {
        return false;
      }
      e.dataTransfer.setData('application/warehouse-stock', this.item.name);
      this.$emit('dragging');
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
  cursor: pointer;
  user-select: none;
  -moz-user-select: none;

  > span {
    position: absolute;
    font-size: .4em;
    text-shadow: 0 0 .5em #000;
    bottom: 0;
    right: 0;
    margin-right: .3em;
    margin-bottom: .2em;
    pointer-events : none;
  }

  > img {
    box-sizing: border-box;

    &:hover {
      background: #ff9f1b;
    }
  }
}
</style>
