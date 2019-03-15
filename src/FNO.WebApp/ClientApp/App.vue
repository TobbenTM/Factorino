<template>
  <div id="app">
    <transition name="el-fade-in">
      <the-preloader v-if="showLoader"/>
    </transition>
    <the-layout v-if="loaded"/>
    <factorio-dialog
      title="Error!"
      class="error-dialog"
      v-if="error"
      v-on:close="errorCleared()"
    >
      <icon :icon="['fas', 'times-circle']" class="error-dialog__icon"/>
      {{ error.toString() }}
    </factorio-dialog>
  </div>
</template>

<script>
import { mapActions, mapState, mapMutations } from 'vuex';
import TheLayout from '@/views/TheLayout';
import ThePreloader from '@/views/ThePreloader';

export default{
  name: 'App',
  components: {
    TheLayout,
    ThePreloader,
  },
  computed: {
    ...mapState('user', [
      'loadedUser',
    ]),
    ...mapState([
      'error',
      'loadedXsrf',
      'loadedLocations',
    ]),
    loaded() {
      return this.loadedUser && this.loadedXsrf && this.loadedLocations;
    },
  },
  data() {
    return {
      showLoader: true,
    };
  },
  watch: {
    loaded(loaded) {
      if (loaded) {
        setTimeout(() => this.showLoader = false, 1000);
      }
    },
  },
  methods: {
    ...mapMutations(['errorCleared']),
    ...mapActions('user', ['loadUser']),
    ...mapActions(['getXsrfToken', 'loadLocations']),
  },
  mounted() {
    this.getXsrfToken();
    this.loadUser();
    this.loadLocations();
  },
}
</script>

<style lang="scss" scoped>
#app {
  height: 100%;
}

.error-dialog {
  &__icon {
    font-size: 4.5em;
    display: block;
    margin: 1rem auto;
    color: #980808;
  }
}
</style>

