<template>
  <div class="header">
    <!-- If the user is logged in, the navbar will show the logo -->
    <span class="header__logo" v-if="!user">Factorino</span>

    <!-- Actions on the right -->
    <a
      class="header__action"
      v-if="!user"
      href="/auth/login"
    >
      <icon :icon="['fab', 'steam']" class="nav-icon"/>
      Log in
    </a>
    <div class="header__action header__user-info" v-else>
      <img :src="user.avatar"/>
      {{ user.name }}
    </div>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';

export default{
  name: 'the-header',
  computed: {
    ...mapState('user', [ 'user' ]),
  },
};
</script>

<style lang="scss" scoped>
.header {
  line-height: 60px;
  margin: 0 1em;

  &__action {
    float: right;
  }

  &__user-info {
    display: flex;
    align-items: center;

    > img {
      border-radius: 50%;
      overflow: hidden;
      margin-right: .5em;
    }
  }
}
</style>
