<template>
  <div>
    <app-spinner v-if="loadingFactory" text="Loading Factory.."/>
  </div>
</template>

<script>
import AppSpinner from '@/components/AppSpinner.vue';

export default{
  name: 'the-factory',
  components: {
    AppSpinner,
  },
  data() {
    return {
      loadingFactory: true,
    };
  },
  mounted() {
    this.loadFactory();
  },
  methods: {
    async loadFactory() {
      this.loadingFactory = true;
    },
  },
}
</script>
