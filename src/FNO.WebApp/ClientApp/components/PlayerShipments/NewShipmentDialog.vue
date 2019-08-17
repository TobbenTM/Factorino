<template>
  <factorio-dialog
    title="New Shipment"
    v-on:close="$emit('close')"
  >
    <div class="shipment__body">
    </div>
  </factorio-dialog>
</template>

<script>
import { mapActions } from 'vuex';

export default {
  components: {
  },
  data() {
    return {
      shipment: {
      },
      creating: false,
    };
  },
  watch: {
  },
  methods: {
    ...mapActions('shipping', ['createShipment']),
    async create() {
      this.creating = true;
      await this.createShipment(this.shipment);
      setTimeout(() => this.$emit('close'), 500);
    }
  },
};
</script>

<style lang="scss" scoped>
.shipment {
  &__body {
    margin: 16px;
    display: grid;
    grid-template-columns: 1fr 2fr;
    grid-gap: 8px;
    align-items: center;
    width: 420px;
  }
}
</style>
