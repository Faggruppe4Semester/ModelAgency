<template>
  <div class="cc col-8">
    <h1>Edit job</h1>
    <div class="row">
      <div class="col-5">
        <table class="table">
          <tbody>
            <tr>
              <td>
                <h5>Customer</h5>
              </td>
              <td>{{job.customer}}</td>
            </tr>
            <tr>
              <td>
                <h5>Start date</h5>
              </td>
              <td>{{job.startDate}}</td>
            </tr>
            <tr>
              <td>
                <h5>Days of work</h5>
              </td>
              <td>{{job.days}}</td>
            </tr>
            <tr>
              <td>
                <h5>Location</h5>
              </td>
              <td>{{job.location}}</td>
            </tr>
            <tr>
              <td>
                <h5>Comments</h5>
              </td>
              <td>{{job.comments}}</td>
            </tr>
            <tr>
              <td>
                <h5>Models</h5>
              </td>
              <td>
                <div class="row m-1" v-for="model in job.models" v-bind:key="model.modelId">
                  <button
                    class="btn btn-danger"
                    v-on:click="removeModelFromJob(model.modelId)"
                    v-if="isManager()"
                  >X</button>
                  <p class="m-1">{{model.firstName}} {{model.lastName}}</p>
                </div>
              </td>
            </tr>
            <tr>
              <td></td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="col-4" v-if="isManager()">
        <form @submit.prevent="addModelToJob">
          <div class="form-group">
            <label>Add model to job</label>
            <select v-model="selected" class="form-control">
              <option
                v-for="model in models"
                :key="model.modelId"
                v-bind:value="model.modelId"
              >{{model.firstName}} {{model.lastName}}</option>
            </select>
          </div>
          <div class="form-group">
            <input type="submit" class="btn btn-success" value="Add model to job" />
          </div>
        </form>
      </div>

      <div class="col-1"></div>
      <div class="col-4" v-if="!isManager()">
        <form @submit.prevent="addExpense">
          <div class="form-group">
            <label for="date">Date</label>
            <input class="form-control" v-model="formEx.date" name="date" />
          </div>
          <div class="form-group">
            <label for="text">Text</label>
            <input class="form-control" v-model="formEx.text" name="text" />
          </div>
          <div class="form-group">
            <label for="amount">Amount</label>
            <input class="form-control" v-model.number="formEx.amount" name="amount" type="number" />
          </div>
          <div class="form-group">
            <input type="submit" value="Add expense to job" class="btn btn-success" />
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: ["id"],
  data() {
    return {
      selected: "",
      models: [],
      job: {},
      formEx: {
        date: "",
        text: "",
        amount: 0
      }
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

      e.PreventDefault();
    },
    addModelToJob(e) {
      var url =
        "https://localhost:44368/api/jobs/" +
        this.job.jobId +
        "/model/" +
        this.selected;
      fetch(url, {
        method: "POST",
        credentials: "include",
        headers: {
          Authorization: "Bearer " + localStorage.getItem("token"),
          "Content-Type": "application/json"
        }
      })
        // eslint-disable-next-line no-console
        .catch(error => () => console.log(error));

      e.PreventDefault();
    },
    removeModelFromJob(id) {
      var url =
        "https://localhost:44368/api/jobs/" + this.job.jobId + "/model/" + id;
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
    },
    isManager() {
      return localStorage.getItem("Role") === "Manager";
    },
    addExpense() {
      var url = "https://localhost:44368/api/expenses";
      fetch(url, {
        method: "POST",
        credentials: "include",
        body: JSON.stringify({
          modelId: localStorage.getItem("id"),
          jobId: this.id,
          date: this.formEx.date,
          text: this.formEx.text,
          amount: this.formEx.amount
        }),
        headers: {
          Authorization: "Bearer " + localStorage.getItem("token"),
          "Content-Type": "application/json"
        }
      }) // eslint-disable-next-line no-console
        .catch(error => () => console.log(error));
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

    url = "https://localhost:44368/api/models/";
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
        this.models = data;
      })
      // eslint-disable-next-line no-console
      .catch(error => () => console.log(error));

    url = "https://localhost:44368/api/expenses/" + this.id;
    fetch(url, {
      method: "GET",
      credentials: "include",
      headers: {
        Authorization: "Bearer " + localStorage.getItem("token"),
        "Content-Type": "application/json"
      }
    });
  }
};
</script>

<style scoped>
.cc {
  margin: 20px;
}
</style>