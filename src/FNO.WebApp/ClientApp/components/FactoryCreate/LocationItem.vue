<template>
  <div
    class="location-item"
    :class="{ 'location-item--active': selected }"
    v-on:click="$emit('click', location)"
  >
    <h1>{{ location.name }}</h1>
    <p>{{ location.description }}</p>
    <ul class="location-item__resource-list">
      <li
        v-for="(resource, index) in location.resources"
        :key="index"
      >
        <factorio-icon :path="mapResource(resource)" />
      </li>
    </ul>
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
  methods: {
    mapResource(resource) {
      switch (resource) {
        case 'coal':
          return 'icons/coal';
        case 'stone':
          return 'icons/stone';
        case 'iron':
          return 'icons/iron-ore';
        case 'copper':
          return 'icons/copper-ore';
        case 'uranium':
          return 'icons/uranium-ore';
        case 'water':
          return 'icons/fluid/water';
        case 'oil':
          return 'icons/fluid/crude-oil';
        default:
          return null;
      }
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

  h1 {
    margin: 0;
  }

  &:hover {
    background: rgba(255, 159, 27, .5);
  }

  &--active {
    border-left: 4px solid rgb(255, 159, 27);
  }

  &__resource-list {
    list-style: none;
    padding: 0;

    > li {
      display: inline;
      margin: 0 8px 0 0;
    }
  }
}
</style>
