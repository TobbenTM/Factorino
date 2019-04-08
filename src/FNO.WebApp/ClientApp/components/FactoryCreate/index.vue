<template>
  <factorio-panel title="New Factory">
    <div class="factory-select">
      <location-list
        v-inlay:light
        class="factory-select__locations"
        :locations="locations"
        v-model="selectedLocation"
      />
      <factory-map
        v-inlay:dark
        class="factory-select__map"
        :locations="locations"
        :selectedLocation="selectedLocation"
        :color="'255, 255, 255'"
      />
      <div class="factory-select__actions">
        <factorio-button
          class="button--success"
          text="Select"
          :small="true"
          :disabled="creating"
          v-on:click="selectLocation"
        />
      </div>
    </div>
    <factorio-dialog
      class="factory-create"
      v-if="creating"
      :dismissable="false"
      title="Creating factory..."
    >
      <ul class="factory-create__state-list">
        <!-- Stream connecting -->
        <step-item :state="pipeline.stream" :text="{
          [State.WAITING]: 'Connect to event stream',
          [State.WORKING]: 'Connecting to event stream...',
          [State.DONE]: 'Connected to event stream!',
          [State.ERROR]: 'Could not connect to event stream!',
        }"/>

        <!-- Creating factory -->
        <step-item :state="pipeline.create" :text="{
          [State.WAITING]: 'Create factory',
          [State.WORKING]: 'Creating factory...',
          [State.DONE]: 'Factory created!',
          [State.ERROR]: 'Could not create factory!',
        }"/>

        <!-- Provisioning factory -->
        <step-item :state="pipeline.provision" :text="{
          [State.WAITING]: 'Provision server',
          [State.WORKING]: 'Provisioning server...',
          [State.DONE]: 'Server provisioned!',
        }"/>

        <!-- Waiting for factory online -->
        <step-item :state="pipeline.online" :text="{
          [State.WAITING]: 'Get endpoint',
          [State.WORKING]: 'Getting endpoint...',
          [State.DONE]: 'Factory online!',
        }"/>
      </ul>
    </factorio-dialog>
  </factorio-panel>
</template>

<script>
import { mapState, mapActions } from 'vuex';
import LocationList from './LocationList';
import StepItem from './StepItem';
import FactoryMap from '@/components/FactoryMap';

const State = {
  WAITING: 'WAITING',
  WORKING: 'WORKING',
  DONE: 'DONE',
  ERROR: 'ERROR',
};

export default {
  components: {
    FactoryMap,
    LocationList,
    StepItem,
  },
  computed: {
    ...mapState([ 'locations' ]),
  },
  data() {
    return {
      State,
      selectedLocation: null,
      creating: false,
      pipeline: {
        stream: State.WAITING,
        create: State.WAITING,
        provision: State.WAITING,
        online: State.WAITING,
      },
    };
  },
  created() {
    this.selectedLocation = this.locations[0];
  },
  methods: {
    ...mapActions('factory', [ 'loadFactories' ]),
    async selectLocation() {
      this.creating = true;
      this.pipeline.stream = State.WORKING;
      this.hub = new this.$signalR.HubConnectionBuilder()
        .withUrl('/ws/factorycreate')
        .configureLogging(this.$signalR.LogLevel.Information)
        .build();

      this.hub.on('ReceiveEvent', this.receiveEvent);
      try {
        await this.hub.start();
        this.pipeline.stream = State.DONE;
        this.createFactory();
      } catch (err) {
        this.pipeline.stream = State.ERROR;
      }
    },
    receiveEvent(evnt, eventType) {
      switch (eventType) {
        case 'FactoryCreatedEvent':
          this.pipeline.create = State.DONE;
          this.pipeline.provision = State.WORKING;
          break;
        case 'FactoryProvisionedEvent':
          this.pipeline.provision = State.DONE;
          this.pipeline.online = State.WORKING;
          break;
        case 'FactoryOnlineEvent':
          this.pipeline.online = State.DONE;
          this.hub.stop();
          setTimeout(() => {
            this.loadFactories();
          }, 1000);
          break;
        default:
          break;
      }
    },
    async createFactory() {
      if (!this.hub) {
        this.pipeline.create = State.ERROR;
      }
      this.pipeline.create = State.WORKING;

      try {
        const result = await this.hub.invoke('CreateFactory', this.selectedLocation.locationId);
      } catch(err) {
        this.pipeline.create = State.ERROR;
      }
    }
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';

.factory-select {
  display: grid;
  grid-template-columns: 2fr 3fr;
  grid-gap: 8px;
  grid-template-areas:
    "locations map"
    "actions actions";

  &__locations {
    grid-area: locations;
  }

  &__map {
    grid-area: map;
  }

  &__actions {
    grid-area: actions;
    display: flex;
    justify-content: end;
  }
}

.factory-create {
  &__state-list {
    margin: 2em;
    padding: 0;
    list-style: none;

    li {
      margin: 4px 0;
    }
  }
}
</style>
