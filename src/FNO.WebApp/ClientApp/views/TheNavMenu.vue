<template>
  <div class="nav-menu">
    <factorio-panel class="nav-menu__panel">
      <div class="nav-menu__actions">
        <factorio-button
          text="World"
          :route="{name: 'world'}"
        />
        <factorio-button
          text="Market"
          :route="{name: 'market'}"
        />
        <factorio-button
          text="Factory"
          :route="{name: 'factory'}"
          :disabled="!user"
        />
        <factorio-button
          text="Player"
          :route="{name: 'player'}"
          :disabled="!user"
        />
      </div>
    </factorio-panel>
  </div>
</template>

<script>
import { mapState } from 'vuex';
export default{
  name: 'the-nav-menu',
  computed: {
    ...mapState('user', ['user']),
  },
};
</script>

<style lang="scss" scoped>
.nav-menu {
  margin: 8px 0;

  &__actions {
    display: grid;
  }
}
</style>
