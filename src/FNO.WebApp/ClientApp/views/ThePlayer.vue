<template>
  <div class="player">
    <factorio-panel v-if="loadingInventory" title="Player">
      <app-spinner
        text="Loading Player.."
        class="player__loader"
      />
    </factorio-panel>
    <div v-else class="player__layout">
      <div class="player__details">
        <factorio-panel title="Player" class="">
          <div class="player__details__container">
            <img :src="user.avatarFull"/>
            <h2>{{ user.name }}</h2>
            <fieldset>
              <legend>Details</legend>
              Credits: {{ user.credits }} $<br>
              Net worth: {{ netWorth }} $
            </fieldset>
          </div>
        </factorio-panel>
      </div>
      <warehouse-inventory
        class="player__warehouse"
        :inventory="remainingInventory"
        v-on:selected="selectedItem = $event"
      />
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from 'vuex';
import WarehouseInventory from '@/components/WarehouseInventory';

export default{
  name: 'the-player',
  components: {
    WarehouseInventory,
  },
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
    netWorth() {
      // TODO: We need to figure out inventory worth
      return this.user.credits;
    },
  },
  data() {
    return {
      trainInventory: [],
      selectedItem: null,
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
  display: flex;
  height: 100%;
  justify-content: center;
  align-items: center;

  &__loader {
    margin: 2em;
  }

  &__layout {
    height: 100%;
    width: 100%;
    display: grid;
    grid-template-areas:
      "details warehouse shipment"
      "details orders shipments";
    grid-template-columns: auto 1fr 1fr;
    grid-template-rows: 1fr 1fr;
    grid-gap: 15px;
    padding: 0 15px;
  }

  &__details {
    grid-area: details;
    margin-top: 15px;

    &__container {
      text-align: center;

      > img {
        border-radius: 50%;
        overflow: hidden;
        margin: 0 1em;
        box-shadow: 0 0 .5em rgba(0, 0, 0, .5);
      }

      > fieldset {
        text-align: left;
      }
    }
  }

  &__warehouse {
    grid-area: warehouse;
    margin-top: 15px;
  }
}
</style>
