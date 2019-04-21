<template>
  <div class="orders">
    <div v-if="loadingOrders" class="orders__loader">
      <app-spinner text="Loading Market.."/>
    </div>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';

export default {
  props: {
    filter: {
      type: Object,
      required: true,
    },
  },
  computed: {
    ...mapState('market', ['orders', 'loadingOrders']),
    pageIndex() {
      return 1; // TODO
    },
  },
  mounted() {
    this.loadOrders(this.filter, this.pageIndex);
  },
  methods: {
    ...mapActions('market', ['loadOrders']),
  },
}
</script>

<style lang="scss" scoped>
.orders {
  &__loader {
    display: flex;
    justify-content: center;
    height: 100%;
  }
}
</style>
