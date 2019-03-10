<template>
  <div id="app">
    <transition name="el-fade-in">
      <the-preloader v-if="showLoader"/>
    </transition>
    <the-layout v-if="loaded"/>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';
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
</style>

