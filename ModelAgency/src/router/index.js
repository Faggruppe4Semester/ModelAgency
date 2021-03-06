import Vue from "vue";
import Router from "vue-router";

import LoginPage from "@/views/Login";
import Dashboard from "@/views/Dashboard";
import Manager from "@/views/Manager";
import EditJob from "@/components/EditJob";
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
      component: LoginPage,
    },
    {
      path: "/dashboard",
      component: Dashboard,
    },
    {
      path: '/editjob',
      name: 'editjob',
      component: EditJob,
      props: true
    },
    {
      path: "/manager",
      component: Manager,
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
