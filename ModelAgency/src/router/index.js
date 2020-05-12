import Vue from "vue";
import Router from "vue-router";

import LoginPage from "@/views/Login";
import Home from "@/components/Home";
Vue.use(Router);

const router = new Router({
  mode: "history",
  routes: [
    {
      path: "*",
      name: "Default",
      redirect: "/login",
    },
    {
      path: "/login",
      name: "Login",
      component: LoginPage,
    },
    {
      path: "/home",
      name: "Home",
      component: Home,
    },
  ],
});

router.beforeEach((to, from, next) => {
  const publicPages = ["/login"];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem("token");

  if (authRequired && !loggedIn) {
    return next("/login");
  }

  return next();
});

export default router;
