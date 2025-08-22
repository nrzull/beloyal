<template>
  <div
    :class="[
      $style.card,
      { [$style.withPadding]: withPadding, [$style.withShadow]: withShadow },
    ]"
  >
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
const { withPadding = true, withShadow = true } = defineProps<{
  withPadding?: boolean;
  withShadow?: boolean;
}>();
</script>

<style module>
:root {
  --card-box-shadow: 0 8px 16px rgba(0, 0, 0, 0.025);
}

.card {
  background-color: var(--color-white);
  border-radius: 8px;
}

.withPadding {
  padding: 32px 24px;
}

.withShadow {
  box-shadow: var(--card-box-shadow);
}
</style>
