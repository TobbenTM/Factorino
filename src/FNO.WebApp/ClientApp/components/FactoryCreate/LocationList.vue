<template>
  <div class="location-list">
    <location-item
      class="location-list__item"
      v-for="(location, index) in locations"
      :key="index"
      :location="location"
      :selected="location === selectedLocation"
      v-on:click="selectedLocation = $event"
    />
  </div>
</template>

<script>
import LocationItem from './LocationItem';

export default {
  props: {
    locations: {
      type: Array,
      required: true,
    },
    value: {
      type: Object,
      required: true,
    }
  },
  components: {
    LocationItem,
  },
  data() {
    return {
      selectedLocation: this.value,
    };
  },
  watch: {
    selectedLocation(newLocation) {
      if (newLocation !== this.value) {
        this.$emit('input', newLocation);
      }
    },
    value(newLocation) {
      if (newLocation !== this.selectedLocation) {
        this.selectedLocation = newLocation;
      }
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
