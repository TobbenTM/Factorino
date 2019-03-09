<template>
  <div id="app">
    <transition name="el-fade-in">
      <the-preloader v-if="!loaded"/>
    </transition>
    <the-layout/>
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
    };
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

