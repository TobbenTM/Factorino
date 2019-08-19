<template>
  <div class="world">
    <div class="world__layout">
        <factorio-panel class="world__highscores">
          <div class="world__panel__content">
            <factorio-panel-header title="Top Players"/>
            <div v-inlay:dark></div>
          </div>
      </factorio-panel>
      <factorio-panel class="world__worldmap">
        <div class="world__panel__content">
          <factorio-panel-header title="World Map"/>
          <div v-inlay:dark></div>
        </div>
      </factorio-panel>
      <factorio-panel class="world__transactions">
        <div class="world__panel__content">
          <factorio-panel-header title="Recent Transactions"/>
          <div v-inlay:dark></div>
        </div>
      </factorio-panel>
    </div>
  </div>
</template>

<script>
export default{
  name: 'the-world',
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

<style lang="scss" scoped>
.world {
  display: flex;
  height: 100%;

  &__layout {
    height: 100%;
    width: 100%;
    display: grid;
    grid-template-areas:
      "highscores worldmap"
      "highscores transactions";
    grid-template-columns: 2fr 5fr;
    grid-template-rows: 3fr 2fr;
    grid-gap: 15px;
    padding: 15px 15px;
    box-sizing: border-box;
  }

  &__highscores {
    grid-area: highscores;
  }

  &__worldmap {
    grid-area: worldmap;
  }

  &__transactions {
    grid-area: transactions;
  }

  &__panel__content {
    height: 100%;
    padding: 0 8px;
    display: grid;
    grid-template-columns: 1fr;
    grid-template-rows: auto 1fr;
    padding-bottom: .3em;
    box-sizing: border-box;
  }
}
</style>
