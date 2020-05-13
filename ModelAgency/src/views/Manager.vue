<template>
  <div class="cc">
    <div class="row">
      <div class="col-2">
        <button
          class="btn btn-secondary btn-block m-1"
          v-on:click="handleButtonPress(3)"
        >Create new model</button>
      </div>
      <div class="col-2">
        <button
          class="btn btn-secondary btn-block m-1"
          v-on:click="handleButtonPress(2)"
        >Create new Manager</button>
      </div>
      <div class="col-2">
        <button
          class="btn btn-secondary btn-block m-1"
          v-on:click="handleButtonPress(1)"
        >Create new job</button>
      </div>
      <div class="col-2">
        <button class="btn btn-secondary btn-block m-1" v-on:click="handleButtonPress(4)">Edit job</button>
      </div>
    </div>
    <div class="row">
      <NewJobForm v-show="show === 1"></NewJobForm>
      <NewManagerForm v-show="show === 2"></NewManagerForm>
      <NewModelForm v-show="show === 3"></NewModelForm>
      <EditJob v-show="show === 4"></EditJob>
    </div>
  </div>
</template>

<script>
import NewJobForm from "@/components/NewJobForm";
import NewManagerForm from "@/components/NewManagerForm";
import NewModelForm from "@/components/NewModelForm";
import EditJob from "@/components/EditJob";
export default {
  data() {
    return {
      show: 0
    };
  },
  components: {
    NewJobForm,
    NewManagerForm,
    NewModelForm,
    EditJob
  },
  methods: {
    handleButtonPress(tab) {
      this.show = tab;
    }
  }
};
</script>

<style scoped>
.cc {
  padding: 5px;
}
</style>