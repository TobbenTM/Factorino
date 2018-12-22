<template>
  <div class="factory">
    <factorino-spinner v-if="loadingFactory" text="Loading Factory.."/>
    <template v-else-if="loadedFactory && !factory">
      <!-- No factory for user -->
      <factory-create class="factory-create"/>
    </template>
    <template v-else-if="loadedFactory && factory">
      <!-- Factory found -->
      <factory-details class="factory-details"/>
    </template>
    <span v-else><icon icon="exclamation-triangle"/> Uh oh, something went wrong!</span>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';
import FactorinoSpinner from 'components/FactorinoSpinner.vue';
import FactoryCreate from 'components/FactoryCreate.vue';
import FactoryDetails from 'components/FactoryDetails.vue';

export default {
  name: 'factory',
  components: {
    FactorinoSpinner,
    FactoryCreate,
    FactoryDetails,
  },
  data() {
    return {

    };
  },
  computed: {
    ...mapState('factory', [
      'factory',
      'loadedFactory',
      'loadingFactory',
    ]),
  },
  methods: {
    ...mapActions('factory', ['loadFactory']),
  },
  mounted() {
    this.loadFactory();
  },
};
</script>

<style lang="scss" scoped>
.factory {
  text-align: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
}
</style>

