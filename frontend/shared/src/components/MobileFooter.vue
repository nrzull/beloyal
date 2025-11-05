<template>
  <div :class="$style.footer">
    <NTabs
      :class="$style.tabs"
      :value="$route.path"
      @update:value="$router.push(getByPath($event))"
    >
      <NTab
        v-for="item in items"
        :key="item.iconVariant"
        :name="typeof item.to === 'string' ? item.to : item.to.path!"
      >
        <Icon :variant="item.iconVariant" />
      </NTab>
    </NTabs>
  </div>
</template>

<script setup lang="ts">
import { NTabs, NTab } from "naive-ui";
import Icon, { type IconVariant } from "./Icon.vue";
import type { RouteLocationRaw } from "vue-router";

const { items } = defineProps<{
  items: Array<{
    to: RouteLocationRaw;

    iconVariant: IconVariant;
  }>;
}>();

const getByPath = (v: string) =>
  items.find((vv) =>
    typeof vv.to === "string" ? vv.to === v : vv.to.path === v
  )!.to;
</script>

<style module>
.footer {
  padding: 0 12px;
  background-color: #fff;
  border-radius: 12px 12px 0 0;
  display: flex;
  justify-content: center;
  align-items: center;
}

.tabs {
  width: max-content;
}
</style>
