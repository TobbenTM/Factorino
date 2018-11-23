<template>
  <div class="player">
    <factorino-spinner text="Loading Player.."/>
  </div>
</template>

<script>
import FactorinoSpinner from 'components/FactorinoSpinner.vue';

export default {
  components: {
    FactorinoSpinner,
  },
  data() {
    return {
    };
  },
};
</script>

<style scoped>
.player {
  text-align: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
}
</style>
