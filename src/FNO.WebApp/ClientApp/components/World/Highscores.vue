<template>
  <factorio-panel>
    <div class="highscores">
      <factorio-panel-header title="Top Players"/>
      <div v-inlay:dark>
        <div
          class="highscores__score"
          v-for="player in highscores"
          :key="player.playerId"
        >
          <img :src="player.avatar" alt="Player avatar"/>
          <h3>{{ player.name }}</h3>
          <p>Credits: {{ player.credits }}$</p>
        </div>
      </div>
    </div>
  </factorio-panel>
</template>

<script>
import { mapState, mapActions } from 'vuex';

export default {
  computed: {
    ...mapState('world', [
      'highscores',
      'loadingHighscores',
    ]),
  },
  mounted() {
    this.loadHighscores();
  },
  methods: {
    ...mapActions('world', ['loadHighscores']),
  },
};
</script>

<style lang="scss">
.highscores {
  height: 100%;
  padding: 0 8px;
  display: grid;
  grid-template-columns: 1fr;
  grid-template-rows: auto 1fr;
  padding-bottom: .3em;
  box-sizing: border-box;

  &__score {
    display: grid;
    grid-template-columns: auto 1fr auto;
    grid-gap: .2em;
    margin: 0 1em;
    align-items: center;

    img {
      border-radius: 50%;
      overflow: hidden;
      margin-right: 8px;
    }
  }
}
</style>
