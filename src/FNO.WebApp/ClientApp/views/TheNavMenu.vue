<template>
  <div class="nav-menu">
    <factorio-panel class="nav-menu__panel">
      <div class="nav-menu__actions">
        <factorio-button
          text="World"
          :route="{name: 'world'}"
        />
        <factorio-button
          text="Factory"
          :route="{name: 'factory'}"
        />
        <factorio-button
          text="Market"
          :route="{name: 'market'}"
        />
        <factorio-button
          text="Player"
          :route="{name: 'player'}"
        />
      </div>
    </factorio-panel>
  </div>
</template>

<script>
export default{
  name: 'the-nav-menu',
}
</script>

<style lang="scss" scoped>
.nav-menu {
  margin: 8px 0;

  &__actions {
    display: grid;
  }
}
</style>
