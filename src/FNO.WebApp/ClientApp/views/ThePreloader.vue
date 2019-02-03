<template>
  <el-container>
    <app-spinner/>
  </el-container>
</template>

<script>
import AppSpinner from '@/components/AppSpinner';

export default{
  name: 'the-preloader',
  components: { AppSpinner },
}
</script>
