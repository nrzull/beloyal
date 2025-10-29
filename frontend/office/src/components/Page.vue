<template>
  <div :class="[$style.page, { [$style.loading]: loading }]">
    <div :class="$style.top">
      <slot name="top"></slot>
    </div>

    <span v-if="loading" :class="$style.loader"></span>

    <slot></slot>
  </div>
</template>

<script setup lang="ts">
defineProps<{ loading?: boolean }>();
</script>

<style module>
.page {
  display: flex;
  flex-flow: column;
  gap: 24px;
  height: inherit;
  position: relative;
}

.page.loading::before {
  content: "";
  position: absolute;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  background-color: var(--background-color);
  z-index: 100;
}

.top {
  display: flex;
  justify-content: space-between;
  gap: 8px;
  align-items: center;
}

.loader {
  position: absolute;
  z-index: 200;
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
}

.loader {
  width: 48px;
  height: 48px;
  border: 5px dotted var(--color-primary);
  border-radius: 50%;
  display: inline-block;
  position: relative;
  box-sizing: border-box;
  animation: rotation 2s linear infinite;
}

@keyframes rotation {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
