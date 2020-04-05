<template>
  <multiselect
    :value="selectedItem"
    @input="$emit('input', $event)"
    label="name"
    track-by="name"
    placeholder="Type to search"
    no-options="No items found"
    select-label=""
    open-direction="bottom"
    :options="items"
    :multiple="false"
    :searchable="true"
    :loading="isLoading"
    :internal-search="false"
    :clear-on-select="false"
    :close-on-select="true"
    :options-limit="300"
    :max-height="420"
    :show-no-results="false"
    :hide-selected="true"
    @search-change="search"
  >
    <template
      slot="singleLabel"
      slot-scope="{ option }">
      <div class="option__desc"><factorio-icon :path="option.icon"/><span class="option__title">{{ option.name | separate | capitalize }}</span></div>
    </template>
    <template
      slot="option"
      slot-scope="{ option }"
    >
      <div class="option__desc"><factorio-icon :path="option.icon"/><span class="option__title">{{ option.name | separate | capitalize }}</span></div>
    </template>
    <span slot="noResult">Oops! No elements found. Consider changing the search query.</span>
  </multiselect>
</template>

<script>
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.min.css';
import '@/css/multi-select.scss';

export default {
  props: {
    value: {
      type: Object,
      required: false,
    },
  },
  components: {
    Multiselect,
  },
  data () {
    return {
      selectedItem: null,
      items: [],
      isLoading: false
    }
  },
  watch: {
    value(newSelectedItem) {
      this.selectedItem = newSelectedItem;
    },
  },
  methods: {
    async search (query) {
      this.isLoading = true;

      try {
        var items = await this.$http.get('/api/items/search', {
          params: {
            query,
          },
        });
        this.items = items.data;
        this.isLoading = false;
      } catch (err) {
        this.isLoading = false;
      }
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
