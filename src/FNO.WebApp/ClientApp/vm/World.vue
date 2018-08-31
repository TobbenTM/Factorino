<template>
  <div class="world">
    <factorino-spinner v-if="loadingWorld" text="Loading World.."/>
  </div>
</template>

<script>
import FactorinoSpinner from 'components/FactorinoSpinner.vue';

export default {
  name: 'world',
  components: {
    FactorinoSpinner,
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
};
</script>

<style scoped>
.world {
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
