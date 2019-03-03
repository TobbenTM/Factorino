<template>
  <div class="layout">
    <the-header class="layout__header"/>
    <the-nav-menu class="layout__nav-menu" v-if="navMenuActive" />
    <router-view class="layout__content" :key="key"/>
  </div>
</template>

<script>
import { mapState } from 'vuex';
import TheHeader from './TheHeader';
import TheNavMenu from './TheNavMenu';

export default{
  name: 'the-layout',
  components: {
    TheHeader,
    TheNavMenu,
  },
  computed: {
    ...mapState(['navMenuActive']),
    key() {
      return this.$route.fullPath;
    },
  },
}
</script>

<style lang="scss" scoped>
.layout {
  height: 100%;
  display: grid;
  grid-template-areas:
    "header header"
    "nav content"
    "toolbar toolbar";
  grid-template-columns: auto 1fr;
  grid-template-rows: auto 1fr auto;

  &__header {
    grid-area: header;
  }

  &__nav-menu {
    grid-area: nav;
  }

  &__content {
    grid-area: content;
  }
}
</style>
