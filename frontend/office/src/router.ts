import { createRouter, createWebHistory } from "vue-router";
import { ensureSelf } from "~/middlewares/ensureSelf";

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/auth",
      component: () => import("~/pages/auth.vue"),
    },

    {
      path: "",
      beforeEnter: [ensureSelf("/auth")],
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
