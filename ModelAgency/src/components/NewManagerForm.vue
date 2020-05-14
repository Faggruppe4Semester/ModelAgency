<template>
  <div class="cc">
    <h1>New Manager</h1>
    <span v-show="errors.length">
      <b>Required:</b>
      <ul>
        <li v-for="error in errors" :key="error.message">{{error.message}}</li>
      </ul>
    </span>
    <form @submit.prevent="checkForm">
      <div class="form-group">
        <label for="firstName">First name</label>
        <input class="form-control" v-model="form.firstName" name="firstName" />
      </div>
      <div class="form-group">
        <label for="lastName">Last name</label>
        <input class="form-control" v-model="form.lastName" name="lastName" />
      </div>
      <div class="form-group">
        <label for="email">Email</label>
        <input class="form-control" v-model="form.email" name="email" />
      </div>
      <div class="form-group">
        <label for="password">Password</label>
        <input class="form-control" v-model="form.password" name="password" type="password" />
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
        firstName: "",
        lastName: "",
        email: "",
        password: ""
      }
    };
  },
  methods: {
    checkForm(e) {
      if (this.firstName && this.lastName && this.email && this.password) {
        let url = "https://localhost:44368/api/managers";
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

      if (!this.form.firstName) {
        this.errors.push({ message: "No first name" });
      }
      if (!this.form.lastName) {
        this.errors.push({ message: "No last name" });
      }
      if (!this.form.email) {
        this.errors.push({ message: "No email" });
      }
      if (!this.form.password) {
        this.errors.push({ message: "No password" });
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