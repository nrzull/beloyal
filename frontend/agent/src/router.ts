import { createRouter, createWebHistory } from "vue-router";

export const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      component: () => import("~/components/DefaultLayout.vue"),
      children: [
        {
          path: "profile",
          component: () => import("~/pages/profile.vue"),
        },
        {
          path: "",
          component: () => import("~/pages/dashboard.vue"),
        },
      ],
    },
  ],
});
