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
          path: "company-accesses",
          children: [
            {
              path: "",
              component: () => import("~/pages/company-accesses.vue"),
            },
          ],
        },

        {
          path: "company-access-invitations",
          children: [
            {
              path: "new",
              component: () =>
                import("~/pages/company-access-invitations/new.vue"),
            },
            {
              path: "",
              component: () => import("~/pages/company-access-invitations.vue"),
            },
          ],
        },

        {
          path: "company-roles",
          children: [
            {
              path: "new",
              component: () => import("~/pages/company-roles/new.vue"),
            },
            {
              path: ":id",
              component: () => import("~/pages/company-roles/_id.vue"),
            },
            {
              path: "",
              component: () => import("~/pages/company-roles.vue"),
            },
          ],
        },

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
