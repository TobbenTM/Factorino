<template>
  <div class="spinner">
    <img src="@/assets/logo-outer.png" class="spin" alt="Loading spinner"/>
    <img src="@/assets/logo-inner.png" class="static" alt="Loading spinner"/>
    <h3 v-if="text">{{ text }}</h3>
  </div>
</template>

<script>
export default {
  props: {
    /**
     * Optional loading text
     */
    text: {
      type: String,
      required: false,
      default: null,
    }
  },
};
</script>

<style lang="scss" scoped>
.spinner {
  position: relative;
  margin: auto;
  width: 256px;
  text-align: center;

  > img {
    height: auto;
    width: auto;
    max-width: 100%;
    left: 0;
    top: 0;

    &.spin {
      animation-name: spin;
      animation-duration: 7500ms;
      animation-iteration-count: infinite;
      animation-timing-function: linear;
    }

    &.static {
      position: absolute;
    }
  }

  > h3 {
    position: relative;
    margin: 0;
    font-weight: 100;
    font-size: 2em;
    user-select: none;
  }
}

@keyframes spin {
  from {transform: rotate(0deg);}
  to {transform: rotate(360deg);}
}
</style>
