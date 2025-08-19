<template>
  <div :class="$style.page">
    <div :class="$style.top">
      <slot name="top"></slot>
    </div>

    <slot></slot>
  </div>
</template>

<script setup lang="ts"></script>

<style module>
.page {
  display: grid;
  gap: 24px;
}

.top {
  display: flex;
  justify-content: space-between;
  gap: 8px;
  align-items: flex-start;
}
</style>
