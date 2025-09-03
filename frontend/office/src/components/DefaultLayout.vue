<template>
  <div :class="$style.default">
    <div :class="$style.aside">
      <Profile />
      <Menu :items="menu" />
    </div>

    <div :class="$style.main">
      <RouterView />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import Profile from "~/components/Profile/Profile.vue";
import Menu from "~/components/Menu/Menu.vue";
import type { MenuItem } from "~/components/Menu/types";
import { admin_white_companies_read, isAdmin } from "~/composables/self";

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
      route: "/mailings",
      label: "Рассылки",
      available: true,
    },

    {
      type: "route",
      route: "/chats",
      label: "Чаты",
      available: true,
    },

    {
      type: "group",
      label: "Персонал",
      available: true,
      children: [
        {
          type: "route",
          route: "/company-access-invitations",
          label: "Приглашения",
          available: true,
        },

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
      label: "Интеграции",
      available: true,
      children: [
        {
          type: "route",
          route: "/telegram",
          label: "Telegram",
          available: true,
        },

        {
          type: "route",
          route: "/max",
          label: "Max",
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
</script>

<style module>
.default {
  display: grid;
  grid-template-areas: "aside main";
  grid-template-columns: max-content 1fr;
  gap: 12px;
  width: inherit;
  height: inherit;
}

.aside {
  grid-area: aside;
  overflow-y: auto;
  overflow-x: clip;
  display: flex;
  flex-flow: column;
  background-color: rgba(0, 0, 0, 0.025);
}

.main {
  grid-area: main;
  overflow-y: auto;
  overflow-x: clip;
  padding: 24px;
  height: 100%;
}
</style>
