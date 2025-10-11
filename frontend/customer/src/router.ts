import { createRouter, createWebHistory } from "vue-router";

export const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/:messenger",
      component: () => import("~/components/DefaultLayout.vue"),
      children: [
        {
          path: "operations",
          component: () => import("~/pages/operations.vue"),
        },
        {
          path: "",
          component: () => import("~/pages/dashboard.vue"),
        },
      ],
    },
  ],
});
