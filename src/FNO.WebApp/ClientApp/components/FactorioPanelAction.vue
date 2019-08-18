<template>
  <a
    href="#"
    v-on:click.prevent="$emit('click')"
    class="action"
  >
    <icon :icon="icon" class="action__icon"/>
  </a>
</template>

<script>
export default {
  props: {
    icon: {
      type: Array,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/variables.scss';

.action {
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

  &.success {
    background: #2d6633;
  }

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

  &:not(:last-child) {
    margin-left: .4em;
  }
}
</style>
