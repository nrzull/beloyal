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
  type?: "primary";
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
}

.button:disabled {
  filter: grayscale(1);
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
  color: #fff;
}

.type-primary:not(:disabled):hover {
  background-color: var(--color-primary-hover);
}
</style>
