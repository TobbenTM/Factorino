<template>
  <div class="corporation">
    <factorino-spinner v-if="loadingCorporation" text="Loading Corporation.."/>
    <template v-else-if="loadedCorporation && !corporation">
      <!-- No corporation for user -->
      <h1>Huh, looks like you don't have a corporation yet!</h1>
      <p>Well, you've got two choices: either start your own or accept a invitation to an existing corporation:</p>
      <div class="new-corp-selection">
        <corporation-create class="corporation-create"/>
        <div class="vertical-rule"/>
        <corporation-invitations class="corporation-invitations"/>
      </div>
    </template>
    <template v-else-if="loadedCorporation && corporation">
      <!-- Corporation found -->
      <corporation-details class="corporation-details"/>
    </template>
    <span v-else><icon icon="exclamation-triangle"/> Uh oh, something went wrong!</span>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';
import FactorinoSpinner from 'components/FactorinoSpinner.vue';
import CorporationCreate from 'components/CorporationCreate.vue';
import CorporationDetails from 'components/CorporationDetails.vue';
import CorporationInvitations from 'components/CorporationInvitations.vue';

export default {
  name: 'corporation',
  components: {
    FactorinoSpinner,
    CorporationCreate,
    CorporationDetails,
    CorporationInvitations,
  },
  data() {
    return {
    };
  },
  computed: {
    ...mapState('corporation', [
      'corporation',
      'loadedCorporation',
      'loadingCorporation',
    ])
  },
  methods: {
    ...mapActions('corporation', ['loadCorporation']),
  },
  mounted() {
    this.loadCorporation();
  },
};
</script>

<style scoped>
.corporation {
  text-align: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
}
.new-corp-selection {
  display: flex;
  align-items: stretch;
  margin-top: 3em;
}
.corporation-create, .corporation-invitations {
  flex-grow: 1;
}
.vertical-rule {
  position: relative;
  flex-grow: 0;
}
.vertical-rule::after {
  content: '';
  width: 1px;
  height: 80%;
  transform: translateY(10%);
  background: rgba(255, 255, 255, .5);
  position: absolute;
}
</style>
