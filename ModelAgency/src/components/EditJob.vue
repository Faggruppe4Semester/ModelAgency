<template>
  <div class="cc col-4">
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
        <input class="form-control" v-model="job.customer" name="customer" />
      </div>
      <div class="form-group">
        <label for="startDate">Start date</label>
        <input class="form-control" v-model="job.startDate" name="startDate" />
      </div>
      <div class="form-group">
        <label for="days">Days of work</label>
        <input class="form-control" v-model.number="job.days" name="days" type="number" />
      </div>
      <div class="form-group">
        <label for="location">Location</label>
        <input class="form-control" v-model="job.location" name="location" />
      </div>
      <div class="form-group">
        <label for="comments">Comments</label>
        <input class="form-control" v-model="job.comments" name="comments" />
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
      job: {}
    };
  },
  methods: {
    checkForm(e) {
      if (
        this.job.customer &&
        this.job.startDate &&
        this.job.days &&
        this.job.location
      ) {
        var url = "https://localhost:44368/api/Jobs/" + this.id;
        fetch(url, {
          method: "PUT",
          credentials: "include",
          body: JSON.stringify(this.job),
          headers: {
            Authorization: "Bearer " + localStorage.getItem("token"),
            "Content-Type": "application/json"
          }
        })
          // eslint-disable-next-line no-console
          .catch(error => () => console.log(error));
      }
      this.errors = [];

      if (!this.job.customer) {
        this.errors.push({ message: "No customer listed" });
      }
      if (!this.job.startDate) {
        this.errors.push({ message: "No start date listed" });
      }
      if (!this.job.days) {
        this.errors.push({ message: "Number of days of work not set" });
      }
      if (Number(this.job.days) <= 0) {
        this.errors.push({
          message: "Number of days of work was not listed or invalid number"
        });
      }
      if (!this.job.location) {
        this.errors.push({ message: "No location listed" });
      }

      e.PreventDefault();
    }
  },
  mounted: function() {
    var url = "https://localhost:44368/api/Jobs/" + this.id;
    fetch(url, {
      method: "GET",
      credentials: "include",
      headers: {
        Authorization: "Bearer " + localStorage.getItem("token"),
        "Content-Type": "application/json"
      }
    })
      .then(responseJson => responseJson.json())
      .then(data => {
        this.job = data;
      })
      // eslint-disable-next-line no-console
      .catch(error => () => console.log(error));
  }
};
</script>

<style scoped>
.cc {
  margin: 20px;
}
</style>