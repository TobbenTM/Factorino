<template>
  <div class="corporation-create">
    <h2>Create corporation</h2>
    <form @submit.prevent="create" class="corporation-form">
      <div class="corporation-form-container">
        <label for="corporation-name">Name:</label>
        <input
          type="text"
          id="corporation-name"
          v-model="corporation.name"
        />
        <label for="corporation-description">Description:</label>
        <textarea
          type="text"
          id="corporation-description"
          v-model="corporation.description"
        />
      </div>
      <vue-ladda
        type="submit"
        :loading="creatingCorporation"
      >
        Create
      </vue-ladda>
    </form>
  </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';

export default {
  data() {
    return {
      corporation: {
        name: '',
        description: '',
      },
    };
  },
  computed: {
    ...mapState('corporation', [
      'creatingCorporation',
    ]),
  },
  methods: {
    ...mapActions('corporation', [
      'createCorporation',
      'loadCorporation',
    ]),
    async create() {
      const result = await this.createCorporation(this.corporation);
      this.loadCorporation();
    },
  },
}
</script>

<style scoped>
.corporation-form {
  min-height: 15em;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.corporation-form-container {
  display: grid;
  margin: 2em 10em;
  padding-bottom: 1em;
  grid-template-columns: 1fr 3fr;
  border-radius: .5em;
  overflow: hidden;
  /* grid-column-gap: 1em; */
  /* grid-row-gap: 1em; */
  color: white;
  background: linear-gradient(to bottom, #303a55 0%,#29334d 39%,#1c2540 100%);
  box-shadow: 0px 15px 50px 0px rgba(0,0,0,0.75);
}
.corporation-form button {
  grid-column-end: span 2;
  background: #1c2540;
}
.corporation-form label {
  text-align: right;
  border-bottom: 2px solid rgba(255, 255, 255, .1);
  padding: 0 1em .5em 1em;
  font-weight: 100;
  justify-self: stretch;
  align-self: end;
  color: rgba(255, 255, 255, .75);
}
.corporation-form input, .corporation-form textarea {
  margin: .5em 0 0 0;
  background: none;
  border: 0;
  outline: 0;
  color: white;
  font-size: 1.1em;
  line-height: 2em;
  resize: none;
  border-bottom: 2px solid rgba(255, 255, 255, .1);
}
</style>
