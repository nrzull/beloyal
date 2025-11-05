<template>
  <div :class="[$style.asideMenuItem, { [$style.active]: active }]">
    <Icon v-if="icon" :variant="icon" />
    {{ label }}
  </div>
</template>

<script setup lang="ts">
import { Icon } from "@beloyal/shared";
import type { IconVariant } from "@beloyal/shared";

defineProps<{ label: string; active: boolean; icon?: IconVariant }>();
</script>

<style module>
.asideMenuItem {
  padding: 12px 0;
  cursor: pointer;
  font-size: 14px;
  color: #111;
  text-decoration: none;
  user-select: none;
  display: flex;
  align-items: center;
  gap: 8px;
}

.active {
  color: var(--color-primary);
}
</style>
