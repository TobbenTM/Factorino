<template>
  <div class="factory-create">
    <h2>Select factory location</h2>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';

export default {
  data() {
    return {
      factory: {
        name: '',
        description: '',
      },
    };
  },
  computed: {
    ...mapState('factory', [
      'creatingFactory',
    ]),
  },
  methods: {
    ...mapActions('factory', [
      'createFactory',
      'loadFactory',
    ]),
    async create() {
      const result = await this.createFactory(this.factory);
      this.loadFactory();
    },
  },
}
</script>

<style lang="scss" scoped>
</style>
