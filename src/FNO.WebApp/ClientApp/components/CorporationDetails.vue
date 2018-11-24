<template>
  <div class="corporation-details">
    <h2>Corporation Details</h2>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';

export default {
  computed: {
    ...mapState('corporation', [
      'corporation',
    ]),
  },
  methods: {
    ...mapActions('corporation', ['leaveCorporation']),
  },
}
</script>

<style scoped>

</style>

