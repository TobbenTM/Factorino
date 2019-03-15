<template>
  <div class="header">
    <!-- Actions on the left -->
    <factorio-panel
      class="header__actions"
    >
      <div class="header__brand">
        <img src="../assets/logo-inverse-layered.png" />
        Factorino
      </div>
      <factorio-button
        v-on:click="navMenuToggled"
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
import { mapState, mapMutations } from 'vuex';

export default{
  name: 'the-header',
  computed: {
    ...mapState('user', [ 'user' ]),
    ...mapState(['navMenuActive']),
  },
  methods: {
    ...mapMutations(['navMenuToggled']),
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

    a {
      vertical-align: text-bottom;
    }
  }

  &__brand {
    display: inline-flex;
    height: 100%;
    align-items: center;
    margin: 0 8px 0 0;

    img {
      max-height: 32px;
      margin-right: 8px;
    }
  }

  &__user {
    grid-area: user;

    a {
      vertical-align: text-bottom;
    }

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
