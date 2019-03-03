<template>
  <factorio-panel title="New Factory">
    <div class="factory-create">
      <location-list
        v-inlay:light
        class="factory-create__locations"
        :locations="locations"
        v-model="selectedLocation"
      />
      <factory-map
        v-inlay:dark
        class="factory-create__map"
        :locations="locations"
        :selectedLocation="selectedLocation"
        :color="'255, 255, 255'"
      />
    </div>
  </factorio-panel>
</template>

<script>
import FactoryMap from '../FactoryMap';
import LocationList from './LocationList';
import Locations from '@/factory-locations';
import Inlay from '@/directives/inlay';

export default {
  components: {
    FactoryMap,
    LocationList,
  },
  directives: {
    Inlay,
  },
  data() {
    return {
      locations: Locations,
      selectedLocation: Locations[0],
    };
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';

.factory-create {
  display: grid;
  grid-template-columns: 2fr 3fr;
  grid-column-gap: 8px;

  // &__locations {
  //   @include panel_inlay_light();
  // }

  // &__map {
  //   @include panel_inlay_dark();
  // }
}
</style>
