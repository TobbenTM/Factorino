<template>
  <factorio-panel title="Market" class="market">
    <div v-if="loadingMarket" class="market__loader">
      <app-spinner text="Loading Market.."/>
    </div>
  </factorio-panel>
</template>

<script>
export default{
  name: 'the-market',
  data() {
    return {
      loadingMarket: true,
    };
  },
  mounted() {
    this.loadMarket();
  },
  methods: {
    async loadMarket() {
      this.loadingMarket = true;
    },
  },
}
</script>

<style lang="scss" scoped>
.market {
  margin: 15px;

  &__loader {
    display: flex;
    justify-content: center;
    height: 100%;
  }
}
</style>
