<template>
  <NConfigProvider :class="$style.provider" :theme-overrides="theme">
    <RouterView />
  </NConfigProvider>
</template>

<script setup lang="ts">
import { NConfigProvider } from "naive-ui";
import { onMounted } from "vue";
import { theme } from "@beloyal/shared";

const onResize = () => {
  const vh = window.innerHeight * 0.01;
  document.documentElement.style.setProperty("--vh1", `${vh}px`);
};

window.addEventListener("resize", onResize);
onMounted(onResize);
</script>

<style>
body {
  height: calc(var(--vh1) * 100);
}
</style>

<style module>
.provider {
  flex-flow: column;
  display: flex;
  height: inherit;
  width: inherit;
  overflow: hidden;
}
</style>
