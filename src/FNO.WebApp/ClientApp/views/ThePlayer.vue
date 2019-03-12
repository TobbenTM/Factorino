<template>
  <factorio-panel title="Player" class="player">
    <div v-if="loadingInventory" class="player__loader">
      <app-spinner text="Loading Player.."/>
    </div>
  </factorio-panel>
</template>

<script>
import { mapState, mapActions } from 'vuex';

export default{
  name: 'the-player',
  computed: {
    ...mapState('user', [
      'user',
      'inventory',
      'loadingInventory'
    ]),
    remainingInventory() {
      if (!this.inventory || this.inventory.length === 0) {
        return [];
      }
      return this.inventory.map(item => {
        const selectedItem = this.trainInventory.find(i => i.itemId == item.itemId);
        if (selectedItem) {
          item.quantity -= selectedItem.quantity;
        }
        return item;
      });
    },
  },
  data() {
    return {
      trainInventory: [],
    };
  },
  mounted() {
    this.loadInventory();
  },
  methods: {
    ...mapActions('user', ['loadInventory']),
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
