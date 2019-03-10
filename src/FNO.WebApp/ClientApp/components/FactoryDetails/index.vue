<template>
  <div class="factory-list">
    <factory-item
      v-for="factory in factories"
      :key="factory.factoryId"
      :factory="factory"
    />
  </div>
</template>

<script>
import { mapState } from 'vuex';
import FactoryItem from './FactoryItem';

export default{
  components: {
    FactoryItem,
  },
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
}
</style>
