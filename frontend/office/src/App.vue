<template>
  <NConfigProvider :class="$style.provider" :theme-overrides="theme">
    <NMessageProvider placement="top-right" closable>
      <RouterView />
    </NMessageProvider>
  </NConfigProvider>
</template>

<script setup lang="ts">
import { NConfigProvider, NMessageProvider } from "naive-ui";
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
