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
      label: "Торговые точки",
      icon: "shop",
      available: true,
    },

    {
      type: "group",
      label: "Аналитика",
      icon: "analytics",
      available: true,
      children: [
        {
          type: "route",
          route: "/analytics",
          label: "Главная",
          available: true,
        },

        {
          type: "route",
          route: "/operations",
          label: "Операции",
          available: true,
        },
      ],
    },

    {
      type: "group",
      label: "Маркетинг",
      available: true,
      icon: "adsClick",
      children: [
        {
          type: "route",
          route: "/bonus-settings",
          label: "Бонусы",
          available: true,
        },

        {
          type: "route",
          route: "/promotions",
          label: "Акции",
          available: true,
        },

        {
          type: "route",
          route: "/levels",
          label: "Уровни",
          available: true,
        },

        {
          type: "route",
          route: "/mailings",
          label: "Рассылки",
          available: true,
        },

        {
          type: "route",
          route: "/qr-stickers",
          label: "QR-Наклейки",
          available: true,
        },
      ],
    },

    {
      type: "route",
      route: "/customers",
      label: "Клиенты",
      available: true,
      icon: "users",
    },

    {
      type: "route",
      route: "/chats",
      label: "Чаты",
      available: true,
      icon: "chats",
    },

    {
      type: "group",
      label: "Персонал",
      available: true,
      icon: "usersSettings",
      children: [
        {
          type: "route",
          route: "/company-accesses",
          label: "Пользователи",
          available: true,
        },

        {
          type: "route",
          route: "/company-roles",
          label: "Роли",
          available: true,
        },
      ],
    },

    {
      type: "group",
      label: "Настройки",
      available: true,
      icon: "settings",
      children: [
        {
          type: "route",
          route: "/company",
          label: "Компания",
          available: true,
        },
        {
          type: "route",
          route: "/telegram",
          label: "Telegram",
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
