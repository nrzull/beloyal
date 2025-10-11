<template>
  <div :class="$style.footer">
    <NTabs :value="$route.path" @update:value="$router.push(getByPath($event))">
      <NTab
        v-for="item in items"
        :key="item.title"
        :name="typeof item.to === 'string' ? item.to : item.to.path!"
      >
        <Space align="center" :size="[4, 0]">
          <Icon :variant="item.iconVariant" />
          {{ item.title }}
        </Space>
      </NTab>
    </NTabs>
  </div>
</template>

<script setup lang="ts">
import { NTabs, NTab } from "naive-ui";
import Icon, { type IconVariant } from "./Icon.vue";
import Space from "./Space";
import type { RouteLocationRaw } from "vue-router";

const { items } = defineProps<{
  items: Array<{
    to: RouteLocationRaw;
    title: string;
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
  padding: 0 24px;
  background-color: #fff;
  border-radius: 16px 16px 0 0;
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 0 auto;
}
</style>
