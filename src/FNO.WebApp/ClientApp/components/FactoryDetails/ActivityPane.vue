<template>
  <div class="activity-pane">
    <table>
      <tbody>
        <tr
          v-for="event in factory.activity"
          :key="event.metadata.offset"
        >
          <td>{{ event.readableEvent }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  props: {
    factory: {
      type: Object,
      required: true,
    },
  },
};
</script>

<style lang="scss" scoped>
.activity-pane {
  padding: 15px;
}
</style>
