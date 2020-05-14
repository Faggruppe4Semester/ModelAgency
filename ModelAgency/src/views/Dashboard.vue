<template>
  <div class="dash col-10">
    <h1>Dashboard</h1>
    <table class="table">
      <thead>
        <tr>
          <th scope="col">Customer</th>
          <th scope="col">Date</th>
          <th scope="col">Location</th>
          <th scope="col">Work days</th>
          <th scope="col">Comments</th>
          <th scope="col">Models</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="job in jobs" :key="job.jobId">
          <td>{{job.customer}}</td>
          <td>{{job.startDate}}</td>
          <td>{{job.location}}</td>
          <td>{{job.days}}</td>
          <td>{{job.comments}}</td>
          <td>
            <p
              v-for="model in job.models"
              :key="model.modelId"
            >{{model.firstName }} {{ model.lastName}}</p>
          </td>
          <td>
            <button class="btn btn-primary" v-on:click="editJob(job.jobId)">Edit</button>
            <button
              class="btn btn-danger m-1"
              v-on:click="deleteJob(job.jobId)"
              v-if="isManager()"
            >Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      jobs: []
    };
  },
  methods: {
    isManager() {
      return localStorage.getItem("Role") === "Manager";
    },
    editJob: function(jobId) {
      this.$router.push({ name: "editjob", params: { id: jobId } });
    },
    deleteJob: function(jobId) {
      var url = "https://localhost:44368/api/Jobs/" + jobId;
      fetch(url, {
        method: "DELETE",
        credentials: "include",
        headers: {
          Authorization: "Bearer " + localStorage.getItem("token"),
          "Content-Type": "application/json"
        }
      })
        // eslint-disable-next-line no-console
        .catch(error => () => console.log(error));
    }
  },
  mounted: function() {
    var url = "https://localhost:44368/api/Jobs/";
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
        if (data.hasOwnProperty("jobId")) {
          this.jobs = { data };
        } else {
          this.jobs = data;
        }
      })
      // eslint-disable-next-line no-console
      .catch(error => () => console.log(error));
  }
};
</script>

<style scoped>
.dash {
  padding: 10px;
}
</style>
