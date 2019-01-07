<template>
  <div class="factory-create">
    <div class="header">
      <h1>Huh, looks like you don't have a factory yet!</h1>
      <p>Better get to work by starting a new one:</p>
      <h2>Select factory location</h2>
    </div>
    <ul class="location-list">
      <li
        v-for="location in locations"
        :key="location.seed"
        class="location-list__item"
        :class="{ 'location-list__item--selected': selectedLocation === location }"
        v-on:click="selectedLocation = location"
      >
        <h1>{{ location.name }}</h1>
        {{ location.description }}
        <div
          class="location-resources"
          :class="{ 'location-resources--selected': selectedLocation === location }"
        >
          <div class="location-resources__resource-name">
            <factorio-icon path="icons/coal" />
          </div>
          <div
            class="location-resources__resource-level"
            :style="{
              backgroundPosition: location.resources.coal * -10 + '%',
            }"
          />
          <div class="location-resources__resource-name">
            <factorio-icon path="icons/stone" />
          </div>
          <div
            class="location-resources__resource-level"
            :style="{
              backgroundPosition: location.resources.stone * -10 + '%',
            }"
          />
          <div class="location-resources__resource-name">
            <factorio-icon path="icons/iron-ore" />
          </div>
          <div
            class="location-resources__resource-level"
            :style="{
              backgroundPosition: location.resources.iron * -10 + '%',
            }"
          />
          <div class="location-resources__resource-name">
            <factorio-icon path="icons/copper-ore" />
          </div>
          <div
            class="location-resources__resource-level"
            :style="{
              backgroundPosition: location.resources.copper * -10 + '%',
            }"
          />
          <div class="location-resources__resource-name">
            <factorio-icon path="icons/fluid/water" />
          </div>
          <div
            class="location-resources__resource-level"
            :style="{
              backgroundPosition: location.resources.water * -10 + '%',
            }"
          />
          <div class="location-resources__resource-name">
            <factorio-icon path="icons/fluid/crude-oil" />
          </div>
          <div
            class="location-resources__resource-level"
            :style="{
              backgroundPosition: location.resources.oil * -10 + '%',
            }"
          />
          <div class="location-resources__resource-name">
            <factorio-icon path="icons/uranium-ore" />
          </div>
          <div
            class="location-resources__resource-level"
            :style="{
              backgroundPosition: location.resources.uranium * -10 + '%',
            }"
          />
          <button
            class="button location-resources__button"
            v-on:click="create"
          >
            Select
          </button>
        </div>
      </li>
    </ul>
    <factory-map
      class="factory-map"
      :locations="locations"
      :selectedLocation="selectedLocation"
    />
    <factory-create-status-modal
      :show="creatingFactory"
      :location="selectedLocation"
      v-on:close="created"
    />
  </div>
</template>

<script>
import { mapActions } from 'vuex';
import FactoryMap from 'components/FactoryMap.vue';
import FactorioIcon from 'components/FactorioIcon.vue';
import FactoryCreateStatusModal from 'components/FactoryCreateStatusModal.vue';
import locations from 'app/factory-locations';

export default {
  components: {
    FactoryMap,
    FactorioIcon,
    FactoryCreateStatusModal,
  },
  data() {
    return {
      locations,
      selectedLocation: locations[0],
      creatingFactory: false,
    };
  },
  methods: {
    ...mapActions('factory', [
      'loadFactory',
    ]),
    create() {
      this.creatingFactory = true;
    },
    created() {
      this.loadFactory();
      this.creatingFactory = false;
    }
  },
}
</script>

<style lang="scss" scoped>
.factory-create {
  display: grid;
  height: 100%;
  grid-template-columns: auto 1fr;
  grid-auto-rows: auto 1fr;
  grid-template-areas:
    "header header"
    "location-list factory-map"
}
.header {
  grid-area: header;
}
.factory-map {
  grid-area: factory-map;
}
.location-list {
  grid-area: location-list;
  align-self: center;
  margin: 3em;
  color: white;
  background: linear-gradient(to bottom, #303a55 0%,#29334d 39%,#1c2540 100%);
  box-shadow: 0px 15px 50px 0px rgba(0,0,0,0.75);
  padding: 1rem 0;
  border-radius: .5em;
  list-style: none;

  &__item {
    white-space: nowrap;
    padding: .75em;
    cursor: pointer;
    text-align: left;
    font-size: .9em;

    &--selected {
      background: rgba(0,0,0,0.1);
      box-shadow: inset 0 0 10px rgba(0,0,0,0.75);
    }

    h1 {
      margin: 0;
      font-weight: 100;
    }
  }
}
.location-resources {
  height: 0;
  margin: 0;
  display: grid;
  overflow: hidden;
  grid-row-gap: .5em;
  grid-column-gap: .5em;
  grid-template-columns: auto 1fr auto 1fr;
  transition: height .3s ease-in-out, margin .3s ease-in-out;

  &--selected {
    height: auto;
    margin-top: 1em;
  }

  &__item {

  }

  &__resource-level {
    align-self: center;

    // Based on the awesome loading bar here: https://codepen.io/zFunx/pen/awzpEB

    width: 100%;
    height: .5em;
    background: repeating-linear-gradient(
        to right,
        #737496,
        #737496 4%,
        transparent 4%,
        transparent 5%
      ),
      repeating-linear-gradient(
        to right,
        #096109,
        #096109 8%,
        transparent 8%,
        transparent 10%
      );

    background-size: 200%, 100%;
    background-position: -100%;
    background-repeat: repeat-y;
  }

  &__button {
    margin: auto;
    grid-column-start: 1;
    grid-column-end: 5;
  }
}
</style>
