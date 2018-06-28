<template>
  <div class="app">
    <div class="nav container sticky">
      <img class="logo" src="../assets/logo-inverse-layered.png" />

      <router-link tag="div" class="nav-link" :to="{ name: 'about' }" exact>
        <icon icon="home" class="nav-icon"/> About
      </router-link>
      <router-link tag="div" class="nav-link" :to="{ name: 'world' }">
        <icon icon="globe" class="nav-icon"/> World
      </router-link>
      <router-link tag="div" class="nav-link" :class="{ disabled: !user }" :to="{ name: 'corporation' }">
        <icon :icon="user ? 'building' : 'lock'" class="nav-icon"/> Corporation
      </router-link>
      <router-link tag="div" class="nav-link" :class="{ disabled: !user }" :to="{ name: 'warehouse' }">
        <icon :icon="user ? 'warehouse' : 'lock'" class="nav-icon"/> Warehouse
      </router-link>
      <router-link tag="div" class="nav-link" :class="{ disabled: !user }" :to="{ name: 'factory' }">
        <icon :icon="user ? 'industry' : 'lock'" class="nav-icon"/> Factory
      </router-link>
      <router-link tag="div" class="nav-link" :class="{ disabled: !user }" :to="{ name: 'market' }">
        <icon :icon="user ? 'coins' : 'lock'" class="nav-icon"/> Marketplace
      </router-link>
      <router-link tag="div" class="nav-link" :to="{ name: 'servers' }">
        <icon icon="server" class="nav-icon"/> Servers
      </router-link>
    </div>
    <div class="chat container sticky">
      <h3>Chat</h3>
    </div>
    <div class="content container">
      <router-view class="current-view"></router-view>
    </div>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';

export default {
  data() {
    return {
      user: false,
    };
  },
};
</script>

<style scoped>
.app {
  height: 100%;
}
h1 {
  font-weight: 100;
}
.container {
}
.sticky.container {
  position: fixed;
  z-index: 1;
  top: 0;
  height: 100%;
  width: 18em;
  overflow-x: hidden;
  color: white;
  background: linear-gradient(to bottom, #303a55 0%,#29334d 39%,#1c2540 100%);
}
.nav {
  left: 0;
  text-align: center;
}
.nav img {
  max-width: 6em;
  margin: 2em 0;
}
.nav-link {
  cursor: pointer;
  color: #748195;
  padding: 1em 2em;
  margin: 1em 0;
  text-align: left;
  font-size: 1.1em;
  transition: color .2s linear;
  border-top: 1px solid rgba(0, 0, 0, 0);
  border-bottom: 1px solid rgba(0, 0, 0, 0);
}
.nav-link.router-link-active, .nav-link.router-link-active:hover {
  color: #ffffff;
  background: rgba(7, 18, 48, .3);
  border-top: 1px solid #030d27;
  border-bottom: 1px solid #030d27;
  box-shadow: inset 0px 11px 40px -10px rgba(14,36,75,0.75),
              inset 0px -11px 40px -10px rgba(14,36,75,0.75);
}
.nav-link:hover {
  color: #acb5c5;
}
.nav-icon {
  margin-right: 1em;
}
.nav-link.disabled {
  color: #505e74;
  cursor: not-allowed;
}
.chat {
  right: 0;
  text-align: center;
}
.current-view {
  width: 100%;
  height: 100%;
}
.content {
  margin: 0 18em;
  display: block;
}
</style>
