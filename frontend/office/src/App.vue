<template>
  <NConfigProvider :class="$style.provider" :theme-overrides="theme">
    <RouterView />
  </NConfigProvider>
</template>

<script setup lang="ts">
import { NConfigProvider } from "naive-ui";
import { theme } from "@beloyal/shared";
</script>

<style module>
.provider {
  flex-flow: column;
  display: flex;
  height: inherit;
  width: inherit;
  overflow: hidden;
}
</style>
