<template>
  <factorio-panel title="Player" class="player">
    <div v-if="loadingPlayer" class="player__loader">
      <app-spinner text="Loading Player.."/>
    </div>
  </factorio-panel>
</template>

<script>
export default{
  name: 'the-player',
  data() {
    return {
      loadingPlayer: true,
    };
  },
  mounted() {
    this.loadPlayer();
  },
  methods: {
    async loadPlayer() {
      this.loadingPlayer = true;
    },
  },
}
</script>

<style lang="scss" scoped>
.player {
  margin: 15px;

  &__loader {
    display: flex;
    justify-content: center;
    height: 100%;
  }
}
</style>
