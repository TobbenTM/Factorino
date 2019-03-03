<template>
  <div class="factory">
    <zoom-center-transition mode="out-in" :duration="200">
      <div v-if="loadingFactory" class="factory__loader">
        <factorio-panel title="Factory">
          <app-spinner text="Loading Factory.."/>
        </factorio-panel>
      </div>
      <factory-create v-else-if="!factory" class="factory__create"/>
      <factory-details v-else class="factory__details"/>
    </zoom-center-transition>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';
import { ZoomCenterTransition } from 'vue2-transitions';
import FactoryCreate from '@/components/FactoryCreate';
import FactoryDetails from '@/components/FactoryDetails';

export default{
  name: 'the-factory',
  components: {
    FactoryCreate,
    FactoryDetails,
    ZoomCenterTransition,
  },
  computed: {
    ...mapState('factory', [
      'factory',
      'loadingFactory',
    ]),
  },
  async created() {
    await this.loadFactory();
  },
  methods: {
    ...mapActions('factory', [
      'loadFactory',
    ]),
  },
}
</script>

<style lang="scss" scoped>
.factory {
  margin: 15px;

  &__loader {
    display: flex;
    height: 100%;
    justify-content: center;
    align-items: center;

    .spinner {
      margin: 2em;
    }
  }
}
</style>
