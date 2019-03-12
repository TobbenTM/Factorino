<template>
  <a
    v-if="!route"
    class="button"
    :class="{
      'button--active': active,
      'button--small': small,
      'button--disabled': disabled,
    }"
    :href="href || '#'"
    :target="target"
    v-on:click="handleClick"
  >
    <slot>{{ text }}</slot>
  </a>
  <router-link
    v-else
    :to="route"
    class="button"
    :class="{
      'button--small': small,
      'button--disabled': disabled,
    }"
    active-class="button--active"
  >
    <slot>{{ text }}</slot>
  </router-link>
</template>

<script>
export default {
  props: {
    /**
     * A route object to navigate to
     */
    route: {
      type: Object,
      required: false,
    },
    /**
     * If the route should match exactly
     */
    exact: {
      type: Boolean,
      required: false,
    },
    /**
     * The text to show on the button
     */
    text: {
      type: String,
      required: false,
    },
    /**
     * Link to hyperlink to if any
     */
    href: {
      type: String,
      required: false,
      default: null,
    },
    /**
     * Target for the anchor
     */
    target: {
      type: String,
      required: false,
      default: null,
    },
    /**
     * Indicates if the button should be shown as active
     */
    active: {
      type: Boolean,
      required: false,
      default: false,
    },
    /**
     * Indicates if the button should be smaller than normal
     */
    small: {
      type: Boolean,
      required: false,
      default: false,
    },
    /**
     * Disable all interactivity
     */
    disabled: {
      type: Boolean,
      required: false,
      default: false,
    },
  },
  methods: {
    handleClick(e) {
      if (this.disabled) return;
      if (!this.href) {
        e.preventDefault();
      }
      this.$emit('click')
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/css/mixins.scss';

.button {
  padding: 15px 30px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  color: white;
  font-weight: 600;
  background: #46464b;
  @include emboss();
  transition: border .1s ease;

  &--success {
    background: #59b664;
    color: black;
  }

  &--danger {
    background: #980808;
    color: white;
  }

  &--active {
    @include inlay();
  }

  &--small {
    padding: 8px 15px;
  }

  &:hover {
    background: #ff9f1b;
    color: black;
  }

  &:active {
    @include inlay();
  }

  &--disabled {
    cursor: not-allowed;
  }

  &--disabled &:hover {
    background: #46464b;
    color: white;
  }
}
</style>

