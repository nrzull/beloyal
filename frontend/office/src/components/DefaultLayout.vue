<template>
  <UserCredentialsModal />

  <div :class="$style.default">
    <div :class="$style.aside">
      <Profile />
      <Menu :items="menu" />
    </div>

    <div v-if="done" :class="$style.main">
      <RouterView />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import Profile from "~/components/Profile.vue";
import Menu from "~/components/Menu.vue";
import type { MenuItem } from "~/components/Menu/types";
import { admin_white_companies_read, isAdmin } from "~/composables/self";
import { wsClient } from "~/api/clients";
import UserCredentialsModal from "~/components/UserCredentials/UserCredentialsModal.vue";
import { useI18n } from "vue-i18n";
import type { CountryMessages } from "~/locales/types";

const { t } = useI18n<{ message: CountryMessages }>();

const adminMenu = computed(() => {
  const base: MenuItem[] = [
    {
      type: "route",
      route: "/white-companies",
      label: "Белый список",
      available: admin_white_companies_read.value,
    },
  ];

  return base.filter((v) => v.available);
});

const companyMenu = computed(() => {
  const base: MenuItem[] = [
    {
      type: "route",
      route: "/shops",
      label: t("shops.menuTitle"),
      icon: "shop",
      available: true,
    },

    {
      type: "group",
      label: t("analytics.menuTitle"),
      icon: "analytics",
      available: true,
      children: [
        {
          type: "route",
          route: "/analytics",
          label: t("analytics.mainMenuTitle"),
          available: true,
        },

        {
          type: "route",
          route: "/operations",
          label: t("analytics.operationsMenuTitle"),
          available: true,
        },
      ],
    },

    {
      type: "group",
      label: t("marketing.menuTitle"),
      available: true,
      icon: "adsClick",
      children: [
        {
          type: "route",
          route: "/bonus-settings",
          label: t("marketing.bonusesMenuTitle"),
          available: true,
        },

        {
          type: "route",
          route: "/promotions",
          label: t("marketing.promotionsMenuTitle"),
          available: true,
        },

        {
          type: "route",
          route: "/levels",
          label: t("marketing.levelsMenuTitle"),
          available: true,
        },

        {
          type: "route",
          route: "/mailings",
          label: t("marketing.mailingsMenuTitle"),
          available: true,
        },

        {
          type: "route",
          route: "/qr-stickers",
          label: t("marketing.qrStickersMenuTitle"),
          available: true,
        },
      ],
    },

    {
      type: "route",
      route: "/customers",
      label: t("clients.menuTitle"),
      available: true,
      icon: "users",
    },

    {
      type: "route",
      route: "/chats",
      label: t("chats.menuTitle"),
      available: true,
      icon: "chats",
    },

    {
      type: "group",
      label: t("staff.menuTitle"),
      available: true,
      icon: "usersSettings",
      children: [
        {
          type: "route",
          route: "/company-accesses",
          label: t("staff.companyAccessesMenuTitle"),
          available: true,
        },

        {
          type: "route",
          route: "/company-roles",
          label: t("staff.rolesMenuTitle"),
          available: true,
        },
      ],
    },

    {
      type: "group",
      label: t("settings.menuTitle"),
      available: true,
      icon: "settings",
      children: [
        {
          type: "route",
          route: "/company",
          label: t("settings.companyMenuTitle"),
          available: true,
        },
        {
          type: "route",
          route: "/telegram",
          label: t("settings.telegramMenuTitle"),
          available: true,
        },
      ],
    },
  ];

  return base.filter((v) => v.available);
});

const menu = computed(() =>
  isAdmin.value ? adminMenu.value : companyMenu.value
);

const done = ref(false);
wsClient.start().then(() => (done.value = true));
</script>

<style module>
.default {
  display: grid;
  grid-template-areas: "aside main";
  grid-template-columns: max-content 1fr;
  width: inherit;
  height: inherit;
}

.aside {
  grid-area: aside;
  overflow-y: auto;
  overflow-x: clip;
  display: flex;
  flex-flow: column;
  border-right: 1px solid rgba(0, 0, 0, 0.075);
  background-color: rgba(255, 255, 255, 0.75);
}

.main {
  grid-area: main;
  overflow-y: auto;
  overflow-x: clip;
  padding: 24px;
  height: 100%;
}
</style>
