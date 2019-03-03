<template>
  <div class="header">
    <!-- Actions on the left -->
    <factorio-panel
      class="header__actions"
    >
      <factorio-button
        v-on:click="toggleNavMenu"
        :active="navMenuActive"
        :small="true"
      >
        <icon :icon="['fas', 'bars']" class="nav-icon"/>
        nav
      </factorio-button>
      <factorio-button
        href="https://github.com/tobbentm/factorino"
        target="_blank"
        :small="true"
      >
        <icon :icon="['fab', 'github']" class="nav-icon"/>
        src
      </factorio-button>
    </factorio-panel>

    <!-- User info on the right -->
    <factorio-panel class="header__user">
      <factorio-button
        v-if="!user"
        href="/auth/login"
        :small="true"
      >
        <icon :icon="['fab', 'steam']" class="nav-icon"/>
        Log in
      </factorio-button>
      <div
        v-else
        class="user-info"
      >
        <img :src="user.avatar"/>
        {{ user.name }}
      </div>
    </factorio-panel>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';

export default{
  name: 'the-header',
  computed: {
    ...mapState('user', [ 'user' ]),
    ...mapState(['navMenuActive']),
  },
  methods: {
    ...mapActions(['toggleNavMenu']),
  },
};
</script>

<style lang="scss" scoped>
.header {
  width: 100%;
  display: grid;
  grid-template-areas:
    "actions void user";
  grid-template-columns: auto 1fr auto;

  &__actions {
    grid-area: actions;
  }

  &__user {
    grid-area: user;

    .user-info {
      display: flex;
      height: 100%;
      align-items: center;
      margin: 0 8px;

      img {
        border-radius: 50%;
        overflow: hidden;
        margin-right: 8px;
      }
    }
  }
}
</style>
