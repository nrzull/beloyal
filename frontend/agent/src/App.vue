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
import { onMounted } from "vue";

const onResize = () => {
  const vh = window.innerHeight * 0.01;
  document.documentElement.style.setProperty("--vh", `${vh}px`);
};

window.addEventListener("resize", onResize);
onMounted(onResize);
</script>

<style>
body {
  height: calc(var(--vh) * 100);
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
