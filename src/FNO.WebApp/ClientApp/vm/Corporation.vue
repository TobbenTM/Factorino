<template>
  <div class="corporation">
    <factorino-spinner v-if="loadingCorporation" text="Loading Corporation.."/>
  </div>
</template>

<script>
import FactorinoSpinner from 'components/FactorinoSpinner.vue';

export default {
  name: 'corporation',
  components: {
    FactorinoSpinner,
  },
  data() {
    return {
      loadingCorporation: true,
    };
  },
  mounted() {
    this.loadCorporation();
  },
  methods: {
    async loadCorporation() {
      this.loadingCorporation = true;
    },
  },
};
</script>

<style scoped>
.corporation {
  text-align: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
}
img.title-logo {
  margin-top: 3em;
  max-width: 10em;
}
h1.title {
  font-weight: 100;
  font-size: 3em;
  margin-top: 0;
}
</style>
