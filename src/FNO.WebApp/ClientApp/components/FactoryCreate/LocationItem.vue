<template>
  <div
    class="location-item"
    :class="{ 'location-item--active': selected }"
    v-on:click="$emit('click', location)"
  >
    <h1>{{ location.name }}</h1>
    <p>{{ location.description }}</p>
  </div>
</template>

<script>
export default {
  props: {
    location: {
      type: Object,
      required: true,
    },
    selected: {
      type: Boolean,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>

.location-item {
  padding: 16px;
  cursor: pointer;
  user-select: none;
  border-left: 4px solid transparent;
  transition: border-left .2s ease;

  &:hover {
    background: rgba(255, 159, 27, .5);
  }

  &--active {
    border-left: 4px solid rgb(255, 159, 27);
  }
}
</style>
