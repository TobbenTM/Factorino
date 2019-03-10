<template>
  <span v-if="state === Creating">
    <icon :icon="['fas', 'spinner']" spin/>
    Creating
  </span>
  <span v-else-if="state === Starting">
    <icon :icon="['fas', 'spinner']" spin/>
    Starting
  </span>
  <span v-else-if="state === Online">
    <icon :icon="['fas', 'check-circle']"/>
    Online
  </span>
  <span v-else>
    <icon :icon="['fas', 'question-circle']"/>
    Unknown
  </span>
</template>

<script>
// Mapped backend enum
const FactoryState = {
  Unknown: 0,
  Creating: 1,
  Starting: 2,
  Online: 3,
};

export default {
  computed: {
  },
  props: {
    /**
     * State value (based on backend enum)
     */
    state: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      ...FactoryState
    };
  },
};
</script>

<style lang="scss" scoped>
</style>
