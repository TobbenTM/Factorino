<template>
  <div class="activity-pane">
    Activity
  </div>
</template>

<script>
export default {
  props: {
    factory: {
      type: Object,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
.activity-pane {
  padding: 15px;
}
</style>
