import Vue from "vue";
import Router from "vue-router";

import LoginPage from "@/views/Login";
import Dashboard from "@/views/Dashboard";
import Manager from "@/views/Manager";
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
      path: "/dashboard",
      name: "Dashboard",
      component: Dashboard,
    },
    {
      path: "/manager",
      name: "Manager",
      component: Manager,
    },
  ],
});

router.beforeEach((to, from, next) => {
  const publicPages = ["/login", "/dashboard", "/manager"];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem("token");

  if (authRequired && !loggedIn) {
    return next("/login");
  }

  return next();
});

export default router;
