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
          path: "customers/:publicToken",
          beforeEnter: [ensureShop("/profile")],
          component: () => import("~/pages/customers/_publicToken.vue"),
          children: [
            {
              path: "add-bonuses",
              component: () =>
                import("~/pages/customers/_publicToken/add-bonuses.vue"),
            },
            {
              path: "subtract-bonuses",
              component: () =>
                import("~/pages/customers/_publicToken/subtract-bonuses.vue"),
            },
            {
              path: "",
              component: () =>
                import("~/pages/customers/_publicToken/actions.vue"),
            },
          ],
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
