<template>
  <div class="error">
    <factorio-panel title="Error!">
      <div class="error__panel">
        <icon :icon="['fas', 'exclamation-triangle']" class="error__icon"/>
        Could not find page!
      </div>
    </factorio-panel>
  </div>
</template>

<script>
export default {
  name: "not-found"
};
</script>

<style lang="scss" scoped>
.error {
  margin: 15px;
  display: flex;
  justify-content: center;
  align-items: center;

  &__panel {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 15px;
  }

  &__icon {
    font-size: 3em;
    display: block;
    margin-bottom: 8px;
  }
}
</style>
