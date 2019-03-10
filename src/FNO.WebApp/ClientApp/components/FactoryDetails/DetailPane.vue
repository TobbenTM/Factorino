<template>
  <fieldset class="detail-pane">
    <legend>Details</legend>
    Id: {{ factory.factoryId }}<br>
    Location: {{ factory.location.name }}<br>
    Owner: {{ factory.owner.name }}<br>
    State: <factory-state :state="factory.state"/><br>
    Endpoint: ??<br>
    Last seen: {{ factory.lastSeen | formatTimestamp }}
  </fieldset>
</template>

<script>
import { formatTimestamp } from '@/filters/datetime';
import FactoryState from '@/components/FactoryState';

export default {
  props: {
    factory: {
      type: Object,
      required: true,
    },
  },
  filters: {
    formatTimestamp,
  },
  components: {
    FactoryState,
  },
};
</script>

<style lang="scss" scoped>

</style>
