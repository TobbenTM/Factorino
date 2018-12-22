<template>
  <img :src="`graphics/${path}.png`" />
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

<style scoped>
img {
  height: 1.5em;
  padding: .2em;
  border-radius: .2em;
  overflow: hidden;
  vertical-align: middle;
  background: rgba(255, 255, 255, .3);
}
</style>

