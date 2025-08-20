<template>
  <div :class="[$style.asideMenuItem, { [$style.active]: active }]">
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
defineProps<{ active?: boolean }>();
</script>

<style module>
.asideMenuItem {
  padding: 12px;
  cursor: pointer;
  font-size: 15px;
  color: #111;
}

.active {
  color: var(--color-primary);
}
</style>
