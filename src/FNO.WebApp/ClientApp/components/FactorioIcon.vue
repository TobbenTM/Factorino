<template>
  <img :src="path" :alt="name" draggable="false"/>
</template>

<script>
export default {
  props: {
    path: {
      required: true,
      type: String,
    },
    name: {
      required: false,
      type: String,
      default: 'Factorio Item',
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';

img {
  height: 1.5em;
  padding: .2em;
  overflow: hidden;
  vertical-align: middle;
}
</style>
