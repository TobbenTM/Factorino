<template>
  <div class="corporation-invitations">
    <h2>Your invitations</h2>
    <div class="invitation-container">
      <template v-if="loadingInvitations">
        <span class="status hint"><icon icon="spinner" spin /> Loading invitations..</span>
      </template>
      <template v-else-if="invitations.length === 0">
        <div>
          <span class="status hint"><icon icon="question-circle"/> No invitations found</span>
          <br/>
          <span class="link hint" @click="loadInvitations"><em><icon icon="sync"/> Reload</em></span>
        </div>
      </template>
      <template v-else-if="invitations.length > 0">
      </template>
      <span v-else><icon icon="exclamation-triangle"/> Uh oh, something went wrong!</span>
    </div>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';

export default {
  computed: {
    ...mapState('user', [
      'invitations',
      'loadingInvitations',
    ])
  },
  methods: {
    ...mapActions('user', ['loadInvitations']),
  },
  mounted() {
    this.loadInvitations();
  },
}
</script>

<style scoped>
.invitation-container {
  background: rgba(0, 0, 0, .15);
  margin: 2em 3em;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 1em;
  min-height: 15em;
  box-shadow: inset 0px 0px 20px 0px rgba(0,0,0,0.5);
}
.hint {
  opacity: .8;
  user-select: none;
  font-weight: 100;
  font-size: 1em;
}
.link {
  cursor: pointer;
  text-decoration: underline dotted rgba(255, 255, 255, .5);
  margin-top: 1em;
  display: block;
}
</style>
