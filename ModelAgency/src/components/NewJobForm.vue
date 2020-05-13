<template>
  <div class="cc">
    <h1>New job</h1>
    <span v-show="errors.length">
      <b>Required:</b>
      <ul>
        <li v-for="error in errors" :key="error.message">{{error.message}}</li>
      </ul>
    </span>
    <form @submit.prevent="checkForm">
      <div class="form-group">
        <label for="customer">Customer</label>
        <input class="form-control" v-model="form.customer" name="customer" />
      </div>
      <div class="form-group">
        <label for="startDate">Start date</label>
        <input class="form-control" v-model="form.startDate" name="startDate" />
      </div>
      <div class="form-group">
        <label for="days">Days of work</label>
        <input class="form-control" v-model.number="form.days" name="days" type="number" />
      </div>
      <div class="form-group">
        <label for="location">Location</label>
        <input class="form-control" v-model="form.location" name="location" />
      </div>
      <div class="form-group">
        <label for="comments">Comments</label>
        <input class="form-control" v-model="form.comments" name="comments" />
      </div>
      <div class="formgroup">
        <input type="submit" value="Create new job" class="btn btn-success" />
      </div>
    </form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      errors: [],
      form: {
        customer: "",
        startDate: "",
        days: 0,
        location: "",
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
        let url = "https://localhost:44368/api/jobs";
        fetch(url, {
          method: "POST",
          credentials: "include",
          body: JSON.stringify(this.form),
          headers: {
            Authorization: "Bearer " + localStorage.getItem("token"),
            "Content-Type": "application/json"
          }
        })
          // eslint-disable-next-line no-console
          .catch(error => () => console.log(error));
      }
      this.errors = [];

      if (!this.form.customer) {
        this.errors.push({ message: "No customer listed" });
      }
      if (!this.form.startDate) {
        this.errors.push({ message: "No start date listed" });
      }
      if (!this.form.days) {
        this.errors.push({ message: "Number of days of work not set" });
      }
      if (Number(this.form.days) <= 0) {
        this.errors.push({
          message: "Number of days of work was not listed or invalid number"
        });
      }
      if (!this.form.location) {
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