import Vue from 'vue';
import VueRouter from 'vue-router';

import About from 'vm/About.vue';
import Corporation from 'vm/Corporation.vue';
import Factory from 'vm/Factory.vue';
import Market from 'vm/Market.vue';
import Servers from 'vm/Servers.vue';
import World from 'vm/World.vue';
import Warehouse from 'vm/Warehouse.vue';
import Player from 'vm/Player.vue';
import AccessDenied from 'vm/AccessDenied.vue';

Vue.use(VueRouter);

const router = new VueRouter({
  mode: 'history',
  routes: [
    {
      name: 'about',
      path: '/',
      component: About,
    },
    {
      name: 'world',
      path: '/world',
      component: World,
    },
    {
      name: 'corporation',
      path: '/corporation',
      component: Corporation,
    },
    {
      name: 'factory',
      path: '/factory',
      component: Factory,
    },
    {
      name: 'market',
      path: '/market',
      component: Market,
    },
    {
      name: 'servers',
      path: '/servers',
      component: Servers,
    },
    {
      name: 'warehouse',
      path: '/warehouse',
      component: Warehouse,
    },
    {
      name: 'player',
      path: '/player',
      component: Player,
    },
    {
      path: '/auth/accessdenied',
      component: AccessDenied,
    },
  ],
});

export default router;
