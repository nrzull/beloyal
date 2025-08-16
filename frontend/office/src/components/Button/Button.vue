<template>
  <button
    :class="[$style.button, $style[`size-${size}`], $style[`type-${type}`]]"
  >
    <slot></slot>
  </button>
</template>

<script setup lang="ts">
const { size = "m", type = "primary" } = defineProps<{
  size?: "m";
  type?: "primary" | "link-primary";
}>();
</script>

<style module>
.button {
  all: unset;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 8px;
  border-radius: 8px;
  font-size: 14px;
  border: 1px solid transparent;
  transition: filter 0.2s ease;
}

.button:disabled {
  filter: grayscale(0.75);
}

.button:not(:disabled):hover {
  cursor: pointer;
}

.size-m {
  padding: 0 12px;
  height: 40px;
}

.type-primary {
  background-color: var(--color-primary);
  color: var(--color-white);
}

.type-primary:not(:disabled):hover {
  background-color: var(--color-primary-hover);
}

.type-link-primary {
  color: var(--color-primary);
  padding: 0;
}

.type-link-primary:not(:disabled):hover {
  color: var(--color-primary-hover);
}
</style>
