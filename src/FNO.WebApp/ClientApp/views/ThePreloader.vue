<template>
  <div class="preloader">
    <app-spinner/>
  </div>
</template>

<script>
import AppSpinner from '@/components/AppSpinner';

export default{
  name: 'the-preloader',
  components: { AppSpinner },
}
</script>

<style lang="scss" scoped>
.preloader {
  position: absolute;
  background: #424242;
  height: 100%;
  width: 100%;
  top: 0;
  left: 0;
  display: flex;
  justify-items: center;
  align-items: center;
}
</style>

