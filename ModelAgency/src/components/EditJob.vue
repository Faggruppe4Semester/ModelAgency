<template>
  <div class="cc col-2">
    <h1>Edit job {{this.id}}</h1>
    <span v-show="errors.length">
      <b>Required:</b>
      <ul>
        <li v-for="error in errors" :key="error.message">{{error.message}}</li>
      </ul>
    </span>
    <form @submit.prevent="checkForm">
      <div class="form-group">
        <label for="customer">Customer</label>
        <input class="form-control" v-model="customer" name="customer" />
      </div>
      <div class="form-group">
        <label for="startDate">Start date</label>
        <input class="form-control" v-model="startDate" name="startDate" />
      </div>
      <div class="form-group">
        <label for="days">Days of work</label>
        <input class="form-control" v-model.number="days" name="days" type="number" />
      </div>
      <div class="form-group">
        <label for="location">Location</label>
        <input class="form-control" v-model="location" name="location" />
      </div>
      <div class="form-group">
        <label for="comments">Comments</label>
        <input class="form-control" v-model="comments" name="comments" />
      </div>
      <div class="formgroup">
        <input type="submit" value="Save job" class="btn btn-success" />
      </div>
    </form>
  </div>
</template>

<script>
export default {
  props: ["id"],
  data() {
    return {
      errors: [],
      form: {
        customer: null,
        startDate: null,
        days: 0,
        location: null,
        comments: ""
      }
    };
  },
  methods: {
    checkForm(e) {
      if (
        this.customer &&
        this.startDate &&
        this.days &&
        this.location &&
        this.comments
      ) {
        return true;
      }
      this.errors = [];

      if (!this.customer) {
        this.errors.push({ message: "No customer listed" });
      }
      if (!this.startDate) {
        this.errors.push({ message: "No start date listed" });
      }
      if (!this.days) {
        this.errors.push({ message: "Number of days of work not set" });
      }
      if (Number(this.days) <= 0) {
        this.errors.push({
          message: "Number of days of work was not listed or invalid number"
        });
      }
      if (!this.location) {
        this.errors.push({ message: "No location listed" });
      }

      e.PreventDefault();
    }
  }
};
</script>

<style scoped>
.cc {
  margin: 20px;
}
</style>