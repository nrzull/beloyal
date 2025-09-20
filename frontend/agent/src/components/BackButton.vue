<template>
  <Space :class="$style.backButton" align="center" :size="[4, 0]">
    <Icon :class="$style.icon" size="m" variant="back" />
    <slot></slot>
  </Space>
</template>

<script setup lang="ts">
import { Icon, Space } from "@beloyal/shared";
</script>

<style module>
.icon {
  color: var(--color-primary);
}

.backButton {
  cursor: pointer;
}
</style>
