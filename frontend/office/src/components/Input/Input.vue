<template>
  <label :class="[$style.input, $style[`size-${size}`]]">
    <input v-bind="$attrs" :class="$style.nativeInput" v-model="model" />
  </label>
</template>

<script setup lang="ts">
defineOptions({ inheritAttrs: false });

const { size = "m" } = defineProps<{ size?: "m" }>();

const model = defineModel<any>();
</script>

<style module>
:root {
  --input-background-color: var(--background-color);
}

.input {
  all: unset;
  display: flex;
  padding: 0 12px;
  border-radius: 8px;
  border: 1px solid rgb(0, 0, 0, 0.01);
  background-color: var(--input-background-color);
}

.nativeInput {
  all: unset;
  width: 100%;
  font-size: 14px;
  height: 100%;
}

.input:has(.nativeInput:not(:disabled):focus) {
  border-color: var(--color-primary);
  background-color: #fff;
}

.size-m {
  height: 40px;
}
</style>
