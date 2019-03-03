<template>
  <li v-if="state === State.WAITING" class="waiting">
    <icon :icon="['fas', 'question-circle']" />
    {{ text[State.WAITING] }}
  </li>
  <li v-else-if="state === State.WORKING" class="working">
    <icon :icon="['fas', 'spinner']" spin />
    {{ text[State.WORKING] }}
  </li>
  <li v-else-if="state === State.DONE" class="done">
    <icon :icon="['fas', 'check']" />
    {{ text[State.DONE] }}
  </li>
  <li v-else-if="state === State.ERROR" class="error">
    <icon :icon="['fas', 'exclamation-triangle']" />
    {{ text[State.ERROR] }}
  </li>
  <li v-else class="unknown">
    <icon :icon="['fas', 'question-circle']" />
    Unknown state!
  </li>
</template>

<script>
const State = {
  WAITING: 'WAITING',
  WORKING: 'WORKING',
  DONE: 'DONE',
  ERROR: 'ERROR',
};

export default {
  props: {
    state: {
      type: String,
      required: true,
    },
    text: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      State,
    };
  },
};
</script>

<style lang="scss" scoped>
.waiting {
  color: #999999;
}
.error {
  color: #f25050;
}
</style>
