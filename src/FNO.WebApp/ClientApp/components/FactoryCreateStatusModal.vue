<template>

</template>

<script>
import Modal from './Modal.vue';

export default {
  props: {
    show: {
      type: Boolean,
      required: true,
    },
    location: {
      type: Object,
      required: true,
    },
  },
  components: {
    Modal,
  },
};
</script>

<style lang="scss" scoped>

</style>
