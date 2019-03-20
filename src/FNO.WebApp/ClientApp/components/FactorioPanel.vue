<template>
  <div class="panel" v-on:click.stop>
    <factorio-panel-header v-if="$slots['action'] || title" :title="title">
      <slot name="action"></slot>
    </factorio-panel-header>
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
}
</style>
