<template>
  <div class="dialog">
    <div class="dialog__mask" v-on:click="close">
      <factorio-panel
        class="dialog__panel"
        :title="title"
      >
        <a
          v-if="dismissable"
          slot="action"
          href="#"
          v-on:click.prevent="close"
          class="dialog__action">
          <icon :icon="['fas', 'times']" class="dialog__action__icon"/>
        </a>
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
@import '@/css/variables.scss';

.dialog {
  &__mask {
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    background: rgba(0, 0, 0, .5);
    display: flex;
    justify-content: center;
    align-items: center;
  }

  &__panel {
    box-shadow: 2px 2px 16px rgba(0, 0, 0, .3);
  }

  &__action {
    color: white;
    border-radius: 4px;
    height: 20px;
    width: 20px;
    display: flex;
    align-items: center;
    justify-content: center;
    overflow: hidden;
    border-top: 2px solid $emboss_dark;
    border-right: 2px solid $emboss_dark;
    border-left: 2px solid $emboss_dark;
    border-bottom: 2px solid $emboss_light;

    &:hover {
      background: #ff9f1b;
      color: black;
    }

    &__icon {
      border-radius: 2px;
      overflow: hidden;
      height: 18px;
      width: 18px;
      font-size: 18px;
      border-top: 2px solid $emboss_light;
    }
  }
}
</style>
