<template>
  <NConfigProvider
    :class="$style.provider"
    :theme-overrides="{
      common: {
        primaryColor: '#0073ff',
        primaryColorHover: '#1c82ff',
        primaryColorPressed: '#0073ff',
        primaryColorSuppl: '#0073ff',
      },
    }"
  >
    <RouterView />
  </NConfigProvider>
</template>

<script setup lang="ts">
import { NConfigProvider } from "naive-ui";
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
