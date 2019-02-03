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
      path: 'world',
      name: 'world',
      component: () => import('@/views/TheWorld'),
    },
    {
      path: 'factory',
      name: 'factory',
      component: () => import('@/views/TheFactory'),
    },
  ],
});

export default router;
