<template>
  <img :src="path" />
</template>

<script>
export default {
  props: {
    path: {
      required: true,
      type: String,
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
  background: #706e70;
  @include emboss();
}
</style>
