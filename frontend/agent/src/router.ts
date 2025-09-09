import { createRouter, createWebHistory } from "vue-router";
import { ensureShop } from "~/middlewares/ensureShop";
import { ensureSelf } from "~/middlewares/ensureSelf";

export const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/auth",
      component: () => import("~/pages/auth.vue"),
    },

    {
      path: "/",
      beforeEnter: [ensureSelf("/auth")],
      component: () => import("~/components/DefaultLayout.vue"),
      children: [
        {
          path: "profile",
          component: () => import("~/pages/profile.vue"),
        },
        {
          path: "",
          beforeEnter: [ensureShop("/profile")],
          component: () => import("~/pages/dashboard.vue"),
        },
      ],
    },
  ],
});
