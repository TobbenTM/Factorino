<template>
  <div class="corporation-details" v-if="corporation">
    <h2>Corporation Details</h2>
    <div class="grid">
      <div class="grid__item grid__item--half">
        <h2 class="corp-name">{{ corporation.name }}</h2>
        <p>{{ corporation.description }}</p>
        <vue-ladda
          @click="leave"
          :loading="leavingCorporation"
          class="button"
        >
          <icon icon="sign-out-alt" />
          Leave Corporation
        </vue-ladda>
      </div>
      <div class="grid__item grid__item--half">
        <h3 class="grid__item__header">
          Members
          <button class="button button--right">
             <icon icon="plus" /> Add member
          </button>
        </h3>
        <ul class="memberlist">
          <li
            v-for="member in corporation.members"
            :key="member.playerId"
            class="memberlist__item"
            :class="{ 'memberlist__item--ceo': corporation.createdByPlayerId === member.playerId }">
            {{ member.name }}
          </li>
        </ul>
      </div>
      <div class="grid__item">
        <h3 class="grid__item__header">Factories</h3>
      </div>
      <div class="grid__item">
        <h3 class="grid__item__header">Warehouse</h3>
      </div>
      <div class="grid__item grid__item--half">
        <h3 class="grid__item__header">Trades</h3>
      </div>
      <div class="grid__item grid__item--half">
        <h3 class="grid__item__header">History</h3>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';

export default {
  computed: {
    ...mapState('corporation', [
      'corporation',
      'leavingCorporation',
    ]),
  },
  methods: {
    ...mapActions('corporation', [
      'leaveCorporation',
      'loadCorporation',
    ]),
    async leave() {
      await this.leaveCorporation(this.corporation);
      this.loadCorporation();
    }
  },
}
</script>

<style lang="scss" scoped>
.grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  grid-gap: 2rem;
  margin: 2rem;

  &__item {
    // per default, an item spans two columns.
    grid-column-end: span 2;
    background: linear-gradient(to bottom, #3f477c 0%,#1a1c3d 100%);
    border-radius: .2em;
    overflow: hidden;
    box-shadow: 0px 10px 40px 0px rgba(0, 0, 0, 0.75);
    padding: 1rem;
    transition: box-shadow .3s ease-in-out;

    &:hover {
      box-shadow: 0px 10px 60px 0px rgba(0, 0, 0, 0.9);
    }

    // smaller items only span one column.
    &--half {
      grid-column-end: span 1;
    }
    // full-width items span the entire row.
    // the numbers here refer to the first and last grid lines.
    &--full {
      grid-column: 1 / -1;
    }

    &__header {
      margin: 0;
      color: rgba(255, 255, 255, .9);
      font-weight: 100;
      padding-bottom: .5rem;
      margin-bottom: .5rem;
      border-bottom: 1px solid rgba(255, 255, 255, .25);
      text-align: left;
    }
  }
}

.memberlist {
  list-style: none;
  padding: 0;

  &__item {

    &--ceo {

    }

    &:hover {

    }
  }
}
</style>
