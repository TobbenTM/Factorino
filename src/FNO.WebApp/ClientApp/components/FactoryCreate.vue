<template>
  <div class="factory-create">
    <h2>Select factory location</h2>
    <div class="dual-pane">
      <ul class="location-list">
        <li
          v-for="location in locations"
          :key="location.seed"
        >
          {{ location.name }}
        </li>
      </ul>
      <factory-map
        class="factory-map"
        :locations="locations"
        :selectedLocation="selectedLocation"
      />
    </div>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';
import FactoryMap from 'components/FactoryMap.vue';
import locations from 'app/factory-locations';

export default {
  components: {
    FactoryMap,
  },
  data() {
    return {
      factory: {
        name: '',
        description: '',
      },
      locations,
      selectedLocation: locations[0],
    };
  },
  computed: {
    ...mapState('factory', [
      'creatingFactory',
    ]),
  },
  methods: {
    ...mapActions('factory', [
      'createFactory',
      'loadFactory',
    ]),
    async create() {
      const result = await this.createFactory(this.factory);
      this.loadFactory();
    },
  },
}
</script>

<style lang="scss" scoped>
.dual-pane {
  display: grid;
  grid-template-columns: 1fr 1fr;
}
.location-list {
  align-self: center;
  margin: 2em;
  color: white;
  background: linear-gradient(to bottom, #303a55 0%,#29334d 39%,#1c2540 100%);
  box-shadow: 0px 15px 50px 0px rgba(0,0,0,0.75);
  padding: 2rem 0;
  border-radius: .5em;
  list-style: none;
}
</style>
