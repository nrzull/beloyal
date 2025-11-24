<template>
  <NConfigProvider :class="$style.provider" :theme-overrides="theme">
    <NMessageProvider placement="top-right" closable>
      <NDialogProvider>
        <RouterView />
      </NDialogProvider>
    </NMessageProvider>
  </NConfigProvider>
</template>

<script setup lang="ts">
import { NConfigProvider, NDialogProvider, NMessageProvider } from "naive-ui";
import { theme } from "@beloyal/shared";
import { useMobileVh } from "@beloyal/shared";

useMobileVh({ variableName: "--vh1" });
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
