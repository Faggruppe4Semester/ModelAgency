<template>
  <div class="center">
    <h1>Login</h1>
    <form>
      <div class="form-group">
        <label for="email">Email</label>
        <input class="form-control" v-model="form.email" name="email" />
      </div>
      <div class="form-group">
        <label for="password">Password</label>
        <input class="form-control" v-model="form.password" name="password" type="password" />
      </div>
      <div class="form-group">
        <button class="btn btn-primary" v-on:click="handleSubmit">Login</button>
      </div>
    </form>
  </div>
</template>

<script>
export default {
  name: "LoginPage",
  data() {
    return {
      form: {
        password: "",
        email: ""
      },
      url: ""
    };
  },
  methods: {
    handleSubmit(e) {
      e.preventDefault();
      fetch("https://localhost:44368/api/account/login", {
        method: "POST",
        body: JSON.stringify(this.form),
        headers: { "Content-Type": "application/json" }
      }).then(res =>
        res.json().then(token => {
          localStorage.setItem("token", token.jwt);
          localStorage.setItem("id", token.id);
          let payload = JSON.parse(window.atob(token.jwt.split(".")[1]));
          localStorage.setItem(
            "Role",
            payload[
              "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            ]
          );
          window.location.href = "/dashboard";
        })
      );
    }
  }
};
</script>

<style scoped>
.center {
  position: absolute;
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
  border: 2px solid gray;
  padding: 50px;
}
</style>
