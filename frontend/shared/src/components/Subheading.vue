<template>
  <div :class="[$style.subheading, $style[type]]">
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
const { type = "" } = defineProps<{ type?: "notice" }>();
</script>

<style module>
.subheading {
  font-size: 14px;
  display: inline-flex;
  align-items: center;
  gap: 8px;
}

.notice {
  color: #999;
}
</style>
