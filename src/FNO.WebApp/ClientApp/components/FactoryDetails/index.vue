<template>
  <div class="factory-list">
    <factorio-panel
      v-for="factory in factories"
      :key="factory.factoryId"
      title="Factory"
      class="factory-list__item"
    >
      {{ factory.factoryId }}
    </factorio-panel>
  </div>
</template>

<script>
import { mapState } from 'vuex';

export default{
  computed: {
    ...mapState('factory', [
      'factories',
    ]),
  },
};
</script>

<style lang="scss" scoped>
.factory-list {
  height: 100%;
  width: 100%;

  &__item {
    display: block;
    margin: 15px;
  }
}
</style>
