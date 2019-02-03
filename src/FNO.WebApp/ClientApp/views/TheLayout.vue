<template>
  <el-container>
    <el-aside v-if="user">
      <the-navbar/>
    </el-aside>
    <el-container>
      <el-header>
        <the-header/>
      </el-header>
      <el-main>
        <router-view :key="key"/>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import { mapActions, mapState } from 'vuex';
import TheHeader from './TheHeader';
import TheNavbar from './TheNavbar';

export default{
  name: 'the-layout',
  components: {
    TheHeader,
    TheNavbar,
  },
  computed: {
    ...mapState('user', [
      'user',
    ]),
    key() {
      return this.$route.fullPath;
    },
  },
}
</script>
