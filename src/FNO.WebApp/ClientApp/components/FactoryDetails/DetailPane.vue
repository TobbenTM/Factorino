<template>
  <fieldset class="detail-pane">
    <legend>Details</legend>
    Id: {{ factory.factoryId }}<br>
    Location: {{ factory.location.name }}<br>
    Owner: {{ factory.owner.name }}<br>
    State: <factory-state :state="factory.state"/><br>
    Endpoint: {{ factory.port | formatEndpoint }}<br>
    Last seen: {{ factory.lastSeen | formatTimestamp }}
    <div class="detail-pane__actions">
      <factorio-button
        class="button--danger"
        :small="true"
        v-on:click="destroyFactory(factory)"
        :disabled="factory.state === FactoryStateEnum.Destroying || factory.state === FactoryStateEnum.Destroyed"
      >
        <template v-if="factory.state === FactoryStateEnum.Destroying">
          <icon :icon="['fas', 'spinner']" spin/> Destroying..
        </template>
        <template v-else>
          <icon :icon="['fas', 'trash-alt']"/> Destroy
        </template>
      </factorio-button>
    </div>
  </fieldset>
</template>

<script>
import { FactoryState as FactoryStateEnum } from '@/enums';
import { formatTimestamp } from '@/filters/datetime';
import FactoryState from '@/components/FactoryState';
import { mapActions } from 'vuex';

export default {
  props: {
    factory: {
      type: Object,
      required: true,
    },
  },
  filters: {
    formatTimestamp,
    formatEndpoint(port) {
      if (!port) return '';
      // TODO: This won't work when we get multiple hosts
      return `${window.location.hostname}:${port}`
    },
  },
  components: {
    FactoryState,
  },
  data() {
    return {
      FactoryStateEnum,
    };
  },
  methods: {
    ...mapActions('factory', [ 'destroyFactory' ]),
  }
};
</script>

<style lang="scss" scoped>
.detail-pane {

  &__actions {
    margin-top: 1em;
    text-align: center;
  }
}
</style>
