import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

const router = new VueRouter({
  mode: 'history',
  routes: [
    {
      path: '',
      redirect: 'world',
    },
    {
      path: '/world',
      name: 'world',
      component: () => import(/* webpackChunkName: "world" */ '@/views/TheWorld'),
    },
    {
      path: '/factory',
      name: 'factory',
      component: () => import(/* webpackChunkName: "factory" */ '@/views/TheFactory'),
    },
    {
      path: '/market',
      name: 'market',
      component: () => import(/* webpackChunkName: "market" */ '@/views/TheMarket'),
    },
    {
      path: '/player',
      name: 'player',
      component: () => import(/* webpackChunkName: "player" */ '@/views/ThePlayer'),
    },
    {
      path: '*',
      component: () => import(/* webpackChunkName: "notfound" */ '@/views/Errors/NotFound'),
    },
  ],
});

export default router;
