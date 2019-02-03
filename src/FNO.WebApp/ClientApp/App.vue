<template>
  <div id="app">
    <transition name="el-fade-in">
      <the-layout v-if="loadedUser"/>
      <the-preloader v-else-if="started"/>
    </transition>
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
    ])
  },
  data() {
    return {
      started: false,
    };
  },
  methods: {
    ...mapActions('user', ['loadUser']),
    ...mapActions(['getXsrfToken']),
  },
  mounted() {
    this.getXsrfToken();
    this.loadUser();
    this.started = true;
  },
}
</script>
