<template>
  <ul>
    <li>
      <router-link class="a" to="/login">Home</router-link>
    </li>
    <li>
      <router-link class="a" to="/dashboard">Dashboard</router-link>
    </li>
    <li v-show="isManager()">
      <router-link class="a" to="/manager">Manager</router-link>
    </li>
  </ul>
</template>

<script>
export default {
  methods: {
    isManager() {
      return localStorage.getItem("Role") === "Manager";
    }
  }
};
</script>

<style scoped>
ul {
  list-style-type: none;
  margin: 0;
  padding: 0;
  overflow: hidden;
  background-color: #333 !important;
}
li {
  float: left;
}

.a {
  display: block;
  color: white;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
}

.a:hover {
  background-color: #111;
}
</style>
