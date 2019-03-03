<template>
  <div class="factory">
    <factorio-panel v-if="loadingFactories" title="Factory">
      <app-spinner
        text="Loading Factory.."
        class="factory__loader"
      />
    </factorio-panel>
    <factory-create v-else-if="!factories || factories.length === 0" class="factory__create"/>
    <factory-details v-else class="factory__details"/>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';
import FactoryCreate from '@/components/FactoryCreate';
import FactoryDetails from '@/components/FactoryDetails';

export default{
  name: 'the-factory',
  components: {
    FactoryCreate,
    FactoryDetails,
  },
  computed: {
    ...mapState('factory', [
      'factories',
      'loadingFactories',
    ]),
  },
  async created() {
    await this.loadFactories();
  },
  methods: {
    ...mapActions('factory', [
      'loadFactories',
    ]),
  },
}
</script>

<style lang="scss" scoped>
.factory {
  display: flex;
  height: 100%;
  justify-content: center;
  align-items: center;

  &__loader {
    margin: 2em;
  }
}
</style>
