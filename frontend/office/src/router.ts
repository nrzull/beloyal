import { createRouter, createWebHistory } from "vue-router";
import { ensureCompanyAccess } from "~/middlewares/ensureCompanyAccess";
import { ensureNoCompanyAccess } from "~/middlewares/ensureNoCompanyAccess";
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
      beforeEnter: [ensureSelf("/auth"), ensureNoCompanyAccess("/")],
      children: [
        {
          path: "company",
          component: () => import("~/pages/welcome/company.vue"),
        },
        {
          path: "invitations",
          component: () => import("~/pages/welcome/invitations.vue"),
        },
        {
          path: "",
          component: () => import("~/pages/welcome.vue"),
        },
      ],
    },

    {
      path: "/",
      beforeEnter: [ensureSelf("/auth"), ensureCompanyAccess("/welcome")],
      component: () => import("~/components/DefaultLayout.vue"),
      children: [
        {
          path: "white-companies",
          children: [
            {
              path: "new",
              component: () => import("~/pages/white-companies/new.vue"),
            },
            {
              path: "",
              component: () => import("~/pages/white-companies.vue"),
            },
          ],
        },

        {
          path: "",
          component: () => import("~/pages/dashboard.vue"),
        },
      ],
    },
  ],
});
