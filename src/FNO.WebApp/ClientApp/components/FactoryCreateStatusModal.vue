<template>
  <modal :show="show" @close="close">
    <p>Creating factory...</p>
  </modal>
</template>

<script>
import Modal from './Modal.vue';

export default {
  props: {
    show: {
      type: Boolean,
      required: true,
    },
    location: {
      type: Object,
      required: true,
    },
  },
  components: {
    Modal,
  },
  data() {
    return {
      hub: null,
    };
  },
  mounted() {
    this.hub = new this.$signalR.HubConnectionBuilder()
      .withUrl("/factorycreate")
      .configureLogging(this.$signalR.LogLevel.Information)
      .build();

    this.hub.on('ReceiveEvent', this.receiveEvent);

    this.hub.start();
  },
  methods: {
    close: function () {
      this.$emit('close');
    },
    receiveEvent(evnt) {
      console.log('Received event: ', evnt);
    },
  },
};
</script>

<style lang="scss" scoped>

</style>
