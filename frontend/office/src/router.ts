import { createRouter, createWebHistory } from "vue-router";
import { ensureCompanyAccess } from "~/middlewares/ensureCompanyAccess";
import { ensureSelf } from "~/middlewares/ensureSelf";

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/auth",
      component: () => import("~/pages/auth.vue"),
    },

    {
      path: "/welcome",
      beforeEnter: [ensureSelf("/auth")],
      component: () => import("~/pages/welcome.vue"),
    },

    {
      path: "",
      beforeEnter: [ensureSelf("/auth"), ensureCompanyAccess("/welcome")],
      component: () => import("~/layouts/Default.vue"),
      children: [
        {
          path: "/",
          component: () => import("~/pages/dashboard.vue"),
        },
      ],
    },
  ],
});
