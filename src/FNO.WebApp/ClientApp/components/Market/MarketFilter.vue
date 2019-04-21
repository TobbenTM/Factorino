<template>
  <div class="filter">
    <label>OrderType</label>
    <label>ItemId</label>
    <label>MinPrice</label>
    <label>MaxPrice</label>
  </div>
</template>

<script>
export default {
  props: {
    value: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {

    };
  },
};
</script>

