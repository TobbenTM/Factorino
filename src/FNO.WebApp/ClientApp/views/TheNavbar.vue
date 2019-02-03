<template>
  <el-menu
    class="navbar"
    :router="true"
    default-active="1"
    background-color="#232B40"
    text-color="#ffffff"
    active-text-color="#ffd04b"
  >
    <img class="navbar__logo" src="@/assets/logo-inverse-layered.png" />
    <el-menu-item :route="{ name: 'world' }" index="1">
      <icon icon="globe" class="el-icon-menu"/>
      World
    </el-menu-item>
    <el-menu-item :route="{ name: 'factory' }" index="2">
      <icon icon="industry" class="el-icon-menu"/>
      Factory
    </el-menu-item>
  </el-menu>
</template>

<script>

export default{
  name: 'the-navbar',
}
</script>

<style lang="scss" scoped>
.navbar {
  border: 0;

  &__logo {
    max-width: 50%;
    margin: 2em auto;
    display: block;
  }
}
</style>

