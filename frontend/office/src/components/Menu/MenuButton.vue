<template>
  <div :class="[$style.asideMenuItem, { [$style.active]: active }]">
    {{ label }}
  </div>
</template>

<script setup lang="ts">
defineProps<{ label: string; active: boolean }>();
</script>

<style module>
.asideMenuItem {
  padding: 12px 36px;
  cursor: pointer;
  font-size: 15px;
  color: #111;
  text-decoration: none;
  user-select: none;
}

.active {
  color: var(--color-primary);
}
</style>
