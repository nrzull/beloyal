<template>
  <RouterLink
    v-slot="{ href, navigate, isActive, isExactActive }"
    :to="item.route"
    custom
  >
    <MenuButton
      :href
      :label="item.label"
      :active="item.exact ? isExactActive : isActive"
      @click="navigate"
      :icon="item.icon"
    />
  </RouterLink>
</template>

<script setup lang="ts">
import MenuButton from "~/components/Menu/MenuButton.vue";
import type { MenuRouteItem } from "~/components/Menu/types";

defineProps<{ item: MenuRouteItem }>();
</script>
