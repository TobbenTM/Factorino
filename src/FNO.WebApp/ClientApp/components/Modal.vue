<template>
  <transition name="modal">
    <div class="modal__mask" @click="close" v-show="show">
      <div class="modal__container" @click.stop>
        <slot></slot>
      </div>
    </div>
  </transition>
</template>

<script>
export default {
  props: {
    show: {
      type: boolean,
      required: true,
    },
  },
  methods: {
    close: function() {
      this.$emit('close');
    },
  },
};
</script>

<style lang="scss" scoped>
.modal {
  &-enter {
    opacity: 0;
  }

  &-enter &__container,
  &-leave-active &__container {
    transform: scale(1.1);
  }

  &-leave-active {
    opacity: 0;
  }

  &__mask {
    position: fixed;
    z-index: 9998;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, .5);
    transition: opacity .3s ease;
  }

  &__container {
    width: 300px;
    margin: 40px auto 0;
    padding: 20px 30px;
    background-color: #fff;
    box-shadow: 0 2px 8px rgba(0, 0, 0, .33);
    transition: all .3s ease;
  }
}
</style>
