<template>
  <div :class="$style.footer">
    <NTabs :value="$route.path" @update:value="$router.push($event)">
      <NTab v-for="item in items" :key="item.title" :name="item.to">
        <NSpace align="center" :size="[4, 0]">
          <Icon :variant="item.iconVariant" />
          {{ item.title }}
        </NSpace>
      </NTab>
    </NTabs>
  </div>
</template>

<script setup lang="ts">
import { NTabs, NTab, NSpace } from "naive-ui";
import Icon, { type IconVariant } from "./Icon.vue";

defineProps<{
  items: Array<{
    to: string;
    title: string;
    iconVariant: IconVariant;
  }>;
}>();
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
