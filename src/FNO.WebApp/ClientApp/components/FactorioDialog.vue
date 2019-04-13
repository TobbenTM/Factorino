<template>
  <div class="dialog">
    <div class="dialog__mask" v-on:click="close">
      <factorio-panel
        class="dialog__panel"
        :title="title"
      >
        <factorio-panel-action
          v-if="dismissable"
          slot="action"
          v-on:click="close"
          :icon="['fas', 'times']"
        />
        <slot></slot>
      </factorio-panel>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    title: {
      type: String,
      required: true,
    },
    dismissable: {
      type: Boolean,
      required: false,
      default: true,
    },
  },
  methods: {
    close() {
      if (!this.dismissable) {
        return;
      }
      this.$emit('close');
    },
  },
};
</script>

<style lang="scss" scoped>
.dialog {
  &__mask {
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    z-index: 1;
    background: rgba(0, 0, 0, .5);
    display: flex;
    justify-content: center;
    align-items: center;
  }

  &__panel {
    box-shadow: 2px 2px 16px rgba(0, 0, 0, .3);
  }
}
</style>
