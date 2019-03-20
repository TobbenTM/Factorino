<template>
  <div class="header">
    <span class="header__title">{{ title }}</span>
    <div class="header__ridges"></div>
    <div class="header__actions">
      <slot></slot>
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
  },
};
</script>

<style lang="scss" scoped>
 .header {
  display: grid;
  grid-template-columns: auto 1fr auto;
  padding-bottom: 10px;

  &__title {
    color: #ffe6c0;
    font-size: 20px;
  }

  &__ridges {

  }
}
</style>
