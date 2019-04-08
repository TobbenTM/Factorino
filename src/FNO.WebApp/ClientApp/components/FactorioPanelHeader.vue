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
    margin: 0 .8em;
    background-size: 6px 100%;
    background-image:
      linear-gradient(90deg, rgba(0,0,0,.2), rgba(255,255,255,.1) 5%, rgba(255,255,255,.1) 45%, rgba(0,0,0,.2) 55%, rgba(0,0,0,.2) 95%, transparent 100%),
      linear-gradient(rgba(200,200,200,.15), transparent 4%, transparent 96%, rgba(0,0,0,.5) 100%);
  }

  &__actions {
    margin-top: -1px;
  }
}
</style>
