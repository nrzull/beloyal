import { createRouter, createWebHistory } from "vue-router";
import { ensureCompanyAccess } from "~/middlewares/ensureCompanyAccess";
import { ensureNoCompanyAccess } from "~/middlewares/ensureNoCompanyAccess";
import { ensureSelf } from "~/middlewares/ensureSelf";

export const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
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
          path: "",
          component: () => import("~/pages/welcome.vue"),
        },
      ],
    },

    {
      path: "/self",
      beforeEnter: [ensureSelf("/auth")],
      component: () => import("~/components/DefaultLayout.vue"),
      children: [
        {
          path: "profile",
          component: () => import("~/pages/self/profile.vue"),
        },

        {
          path: "company-accesses",
          component: () => import("~/pages/self/company-accesses.vue"),
        },
      ],
    },

    {
      path: "/",
      beforeEnter: [ensureSelf("/auth"), ensureCompanyAccess("/welcome")],
      component: () => import("~/components/DefaultLayout.vue"),
      children: [
        {
          path: "telegram",
          component: () => import("~/pages/telegram.vue"),
          children: [
            {
              path: "",
              component: () => import("~/pages/telegram/_id.vue"),
            },
          ],
        },
        {
          path: "chats",
          children: [
            {
              path: "",
              component: () => import("~/pages/chats.vue"),
            },
          ],
        },
        {
          path: "mailings",
          children: [
            {
              path: "new",
              component: () => import("~/pages/mailings/new.vue"),
            },
            {
              path: "",
              component: () => import("~/pages/mailings.vue"),
            },
          ],
        },
        {
          path: "operations",
          component: () => import("~/pages/operations.vue"),
        },
        {
          path: "bonus-settings",
          component: () => import("~/pages/bonus-settings.vue"),
        },
        {
          path: "promotions",
          children: [
            {
              path: "new",
              component: () => import("~/pages/promotions/new.vue"),
            },

            {
              path: "",
              component: () => import("~/pages/promotions.vue"),
            },
          ],
        },
        {
          path: "company-accesses",
          children: [
            {
              path: "new",
              component: () => import("~/pages/company-accesses/new.vue"),
            },
            {
              path: ":id",
              component: () => import("~/pages/company-accesses/_id.vue"),
            },
            {
              path: "",
              component: () => import("~/pages/company-accesses.vue"),
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
              path: ":id",
              component: () => import("~/pages/white-companies/_id.vue"),
            },
            {
              path: "",
              component: () => import("~/pages/white-companies.vue"),
            },
          ],
        },

        {
          path: "shops",
          children: [
            {
              path: "new",
              component: () => import("~/pages/shops/new.vue"),
            },
            {
              path: ":id",
              component: () => import("~/pages/shops/_id.vue"),
              children: [
                {
                  path: "promotions",
                  component: () => import("~/pages/shops/_id/promotions.vue"),
                },

                {
                  path: "",
                  component: () =>
                    import("~/pages/shops/_id/company-accesses.vue"),
                },
              ],
            },
            {
              path: "",
              component: () => import("~/pages/shops.vue"),
            },
          ],
        },

        {
          path: "qr-stickers",
          component: () => import("~/pages/qr-stickers.vue"),
        },

        {
          path: "",
          component: () => import("~/pages/redirect.vue"),
        },
      ],
    },
  ],
});
