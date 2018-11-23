<template>
  <div class="access-denied">
    <img class="title-logo" src="../assets/logo-inverse-layered.png" />
    <h1 class="title">Oops!</h1>
    <p>It seems you don't have the required access to view this resource!</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
    };
  },
};
</script>

<style scoped>
.access-denied {
  text-align: center;
}
img.title-logo {
  margin-top: 3em;
  max-width: 10em;
}
h1.title {
  font-weight: 100;
  font-size: 3em;
  margin-top: 0;
}
</style>
