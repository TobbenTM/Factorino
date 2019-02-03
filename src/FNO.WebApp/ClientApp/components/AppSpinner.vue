<template>
  <div class="spinner">
    <img src="@/assets/logo-outer.png" class="spin"/>
    <img src="@/assets/logo-inner.png"/>
    <h3 v-if="text">{{ text }}</h3>
  </div>
</template>

<script>
export default {
  props: ['text'],
  data() {
    return {

    };
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
    position: absolute;
    left: 0;
    top: 0;
  }
  > img.spin {
    animation-name: spin;
    animation-duration: 7500ms;
    animation-iteration-count: infinite;
    animation-timing-function: linear;
  }
  > h3 {
    position: relative;
    margin: 0;
    padding-top: 256px;
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
