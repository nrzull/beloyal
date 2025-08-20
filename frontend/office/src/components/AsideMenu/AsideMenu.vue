<template>
  <aside :class="$style.aside">
    <RouterLink
      v-for="item in menu"
      :key="item.title"
      :to="item.link"
      custom
      v-slot="{ navigate, isActive, isExactActive }"
    >
      <AsideMenuItem
        @click="navigate"
        :active="item.exact ? isExactActive : isActive"
      >
        {{ item.title }}
      </AsideMenuItem>
    </RouterLink>
  </aside>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { isAdmin, admin_white_companies_read } from "~/composables/self";
import type { TAsideMenuItem } from "~/components/AsideMenu/types";
import AsideMenuItem from "~/components/AsideMenu/AsideMenuItem.vue";

const adminMenu = computed(() => {
  const base: TAsideMenuItem[] = [
    {
      link: "/",
      title: "Главная",
      available: true,
      exact: true,
    },

    {
      link: "/white-companies",
      title: "Белый список",
      available: admin_white_companies_read.value,
    },

    {
      link: "/auth",
      title: "Выйти",
      available: true,
    },
  ];

  return base.filter((v) => v.available);
});

const companyMenu = computed(() => {
  const base: TAsideMenuItem[] = [
    {
      link: "/",
      title: "Главная",
      available: true,
      exact: true,
    },

    {
      link: "/shops",
      title: "Торговые точки",
      available: true,
    },

    {
      link: "/promotions",
      title: "Акции",
      available: true,
    },

    {
      link: "/mailings",
      title: "Рассылки",
      available: true,
    },

    {
      link: "/customers",
      title: "Клиенты",
      available: true,
    },

    {
      link: "/chats",
      title: "Чаты",
      available: true,
    },

    {
      link: "/integrations",
      title: "Интеграции",
      available: true,
    },

    {
      link: "/company-accesses",
      title: "Пользователи",
      available: true,
    },

    {
      link: "/company-roles",
      title: "Роли",
      available: true,
    },

    {
      link: "/company-access-invititations",
      title: "Приглашения",
      available: true,
    },

    {
      link: "/auth",
      title: "Выйти",
      available: true,
    },
  ];

  return base.filter((v) => v.available);
});

const menu = computed(() =>
  isAdmin.value ? adminMenu.value : companyMenu.value
);
</script>

<style module>
.aside {
  padding: 12px 0;
  background-color: rgba(0, 0, 0, 0.025);
}
</style>
