<template>
  <div>
    <app-spinner v-if="loadingWorld" text="Loading World.."/>
  </div>
</template>

<script>
import AppSpinner from '@/components/AppSpinner.vue';

export default{
  name: 'the-world',
  components: {
    AppSpinner,
  },
  data() {
    return {
      loadingWorld: true,
    };
  },
  mounted() {
    this.loadWorld();
  },
  methods: {
    async loadWorld() {
      this.loadingWorld = true;
    },
  },
}
</script>
