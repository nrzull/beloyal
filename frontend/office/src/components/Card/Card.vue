<template>
  <div :class="[$style.card, { [$style.withPadding]: withPadding }]">
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
const { withPadding = true } = defineProps<{ withPadding?: boolean }>();
</script>

<style module>
.card {
  background-color: var(--color-white);
  border-radius: 8px;
  box-shadow: var(--box-shadow-card);
}

.withPadding {
  padding: 32px 24px;
}
</style>
