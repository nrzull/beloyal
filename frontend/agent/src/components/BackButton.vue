<template>
  <NSpace :class="$style.backButton" align="center" :size="[4, 0]">
    <Icon :class="$style.icon" size="m" variant="back" />
    <slot></slot>
  </NSpace>
</template>

<script setup lang="ts">
import { Icon } from "@beloyal/shared";
import { NSpace } from "naive-ui";
</script>

<style module>
.icon {
  color: var(--color-primary);
}

.backButton {
  cursor: pointer;
}
</style>
