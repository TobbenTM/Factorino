<template>
  <div class="player">
    <!-- <factorio-panel v-if="loading" title="Player">
      <app-spinner
        text="Loading Player.."
        class="player__loader"
      />
    </factorio-panel> -->
    <div class="player__layout">
      <div class="player__details">
        <factorio-panel title="Player">
          <div class="player__details__container">
            <img :src="user.avatarFull" alt="Player avatar"/>
            <h2>{{ user.name }}</h2>
            <fieldset>
              <legend>Details</legend>
              Credits: {{ user.credits }} $<br>
              Net worth: {{ netWorth }} $<br>
              Factorio Username: {{ user.factorioId || 'Missing!' }}
            </fieldset>
          </div>
        </factorio-panel>
      </div>
      <player-inventory
        class="player__warehouse"
        v-on:selected="selectedItem = $event"
      />
      <player-orders class="player__orders"/>
      <player-shipments class="player__shipments"/>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from 'vuex';
import PlayerInventory from '@/components/PlayerInventory';
import PlayerShipments from '@/components/PlayerShipments';
import PlayerOrders from '@/components/PlayerOrders';

export default{
  name: 'the-player',
  components: {
    PlayerInventory,
    PlayerShipments,
    PlayerOrders,
  },
  computed: {
    ...mapState('user', ['user']),
    netWorth() {
      // TODO: We need to figure out inventory worth
      return this.user.credits;
    },
  },
  data() {
    return {
      selectedItem: null,
    };
  },
}
</script>

<style lang="scss" scoped>
.player {
  display: flex;
  height: 100%;

  &__loader {
    margin: 2em;
  }

  &__layout {
    height: 100%;
    width: 100%;
    display: grid;
    grid-template-areas:
      "details warehouse shipments"
      "details warehouse orders";
    grid-template-columns: auto 1fr 1fr;
    grid-template-rows: 1fr 1fr;
    grid-gap: 15px;
    padding: 15px 15px;
    box-sizing: border-box;
  }

  &__details {
    grid-area: details;

    &__container {
      text-align: center;
      padding-top: 1em;

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
  }

  &__orders {
    grid-area: orders;
  }

  &__shipments {
    grid-area: shipments;
  }
}
</style>
