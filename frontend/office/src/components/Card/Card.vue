<template>
  <div :class="[$style.card, { [$style.withPadding]: withPadding }]">
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
const { withPadding = true } = defineProps<{ withPadding?: boolean }>();
</script>

<style module>
:root {
  --card-box-shadow: 0 8px 16px rgba(0, 0, 0, 0.025);
}

.card {
  background-color: var(--color-white);
  border-radius: 8px;
  box-shadow: var(--card-box-shadow);
}

.withPadding {
  padding: 32px 24px;
}
</style>
