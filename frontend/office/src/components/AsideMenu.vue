<template>
  <aside v-if="isAdmin" :class="$style.aside">
    <RouterLink
      v-for="item in menu"
      :key="item.title"
      :to="item.link"
      custom
      v-slot="{ navigate }"
    >
      <Button @click="navigate" type="link-primary">
        {{ item.title }}
      </Button>
    </RouterLink>
  </aside>
</template>

<script setup lang="ts">
import { computed } from "vue";
import type { RouteLocationRaw } from "vue-router";
import { isAdmin, admin_white_companies_read } from "~/composables/self";
import Button from "~/components/Button/Button.vue";

type MenuItem = { link: RouteLocationRaw; title: string; available: boolean };

const adminMenu = computed(() => {
  const base: MenuItem[] = [
    {
      link: "/",
      title: "Главная",
      available: true,
    },

    {
      link: "/white-companies",
      title: "Белый список",
      available: admin_white_companies_read.value,
    },
  ];

  return base.filter((v) => v.available);
});

const companyMenu = computed(() => {
  const base: MenuItem[] = [
    {
      link: "/",
      title: "Главная",
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
}
</style>
