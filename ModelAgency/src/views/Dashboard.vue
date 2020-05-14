<template>
  <div class="dash">
    <h1>Dashboard</h1>
    <table>
      <tr>
        <th>Customer</th>
        <th>Date</th>
        <th>Location</th>
        <th>Work days</th>
        <th>Comments</th>
        <th>Models</th>
        <th v-if="isManager"></th>
      </tr>
      <tr v-for="job in jobs" :key="job.jobId">
        <td>{{job.customer}}</td>
        <td>{{job.startDate}}</td>
        <td>{{job.location}}</td>
        <td>{{job.days}}</td>
        <td>{{job.comments}}</td>
        <td>
          <span
            v-for="jobModel in job.jobModels"
            :key="jobModel.efModelId"
          >{{jobModel.model.firstName }} {{ jobModel.model.lastName}}</span>
          <br />
        </td>
        <td v-if="isManager">
          <button class="btn btn-primary" v-on:click="editJob(job.efJobId)">Edit</button>
        </td>
      </tr>
    </table>
    <ul>
      <li v-for="job in jobs" :key="job.jobId">{{job}}</li>
    </ul>
  </div>
</template>

<script>
export default {
  data() {
    return {
      jobs: []
    };
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
  },
  methods: {
    isManager: function() {
      if (localStorage.getItem("Role") === "Manager") {
        return true;
      } else {
        return false;
      }
    },
    editJob: function(jobId) {
      this.$router.push({ name: "editjob", params: { id: jobId } });
    }
  }
};
</script>

<style scoped>
.dash {
  padding: 10px;
}
table,
th,
td {
  border: 1px solid black;
  padding: 15px;
}
</style>
