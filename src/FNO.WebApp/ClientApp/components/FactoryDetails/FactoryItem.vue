<template>
  <factorio-panel
    title="Factory"
    class="item"
  >
    <div class="item__content">
      <detail-pane
        :factory="factory"
        class="item__content__detail"
      />
      <factory-map
        v-inlay:dark
        class="item__content__map"
        :locations="[factory.location]"
        :selectedLocation="factory.location"
        :color="'255, 255, 255'"
      />
      <activity-pane
        v-inlay:light
        :factory="factory"
        class="item__content__activity"
      />
    </div>
  </factorio-panel>
</template>

<script>
import FactoryMap from '@/components/FactoryMap';
import Inlay from '@/directives/inlay';
import DetailPane from './DetailPane';
import ActivityPane from './ActivityPane';

export default {
  props: {
    factory: {
      type: Object,
      required: true,
    },
  },
  directives: {
    Inlay,
  },
  components: {
    FactoryMap,
    DetailPane,
    ActivityPane,
  },
};
</script>

<style lang="scss" scoped>
.item {
  display: block;
  margin: 15px;

  &__content {
    display: grid;
    grid-template-areas:
      "details map activity";
    grid-template-columns: auto auto 1fr;
    grid-gap: 15px;

    &__details {
      grid-area: details;
    }

    &__map {
      grid-area: map;
    }

    &__activity {
      grid-area: activity;
    }
  }
}
</style>
