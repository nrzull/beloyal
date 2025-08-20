<template>
  <button
    :class="[$style.button, $style[`size-${size}`], $style[`type-${type}`]]"
  >
    <slot></slot>
  </button>
</template>

<script setup lang="ts">
const { size = "l", type = "primary" } = defineProps<{
  size?: "l" | "m";
  type?: "primary" | "link-primary" | "outline-primary";
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

  border: 1px solid transparent;
  transition: filter 0.2s ease;
}

.button:disabled {
  filter: grayscale(0.75);
}

.button:not(:disabled):hover {
  cursor: pointer;
}

.size-l {
  padding: 0 16px;
  height: 40px;
  font-size: 14px;
}

.size-m {
  padding: 0 16px;
  height: 32px;
  font-size: 14px;
}

.type-outline-primary {
  border-color: var(--color-primary);
  color: var(--color-primary);
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
