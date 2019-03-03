<template>
  <div class="panel" v-on:click.stop>
    <div class="panel__header" v-if="$slots['action'] || title">
      <span class="title">{{ title }}</span>
      <div class="actions">
        <slot name="action"></slot>
      </div>
    </div>
    <slot></slot>
  </div>
</template>

<script>
export default {
  props: {
    /**
     * Optionally sets a title for the panel
     */
    title: {
      type: String,
      required: false,
      default: null,
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';

.panel {
  padding: 8px;
  display: inline-block;
  font-weight: 600;
  background: #313031;
  @include emboss();

  &__header {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20px;

    .title {
      color: #ffe6c0;
      font-size: 20px;
    }
  }
}
</style>
