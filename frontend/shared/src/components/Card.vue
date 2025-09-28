<template>
  <div
    :class="[
      $style.card,
      {
        [$style.withPadding]: withPadding,
        [$style.withShadow]: withShadow,
        [$style.withBorder]: withBorder,
      },
    ]"
  >
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
const {
  withPadding = true,
  withShadow = true,
  withBorder = false,
} = defineProps<{
  withPadding?: boolean;
  withShadow?: boolean;
  withBorder?: boolean;
}>();
</script>

<style module>
:root {
  --card-box-shadow: 0 8px 16px rgba(0, 0, 0, 0.025);
}

.card {
  background-color: var(--color-white);
  border-radius: 8px;
  border: 1px solid transparent;
}

.withPadding {
  padding: 32px 24px;
}

.withShadow {
  box-shadow: var(--card-box-shadow);
}

.withBorder {
  border-color: rgba(0, 0, 0, 0.1);
}
</style>
