<template>
  <div :class="$style.previews">
    <img v-for="img in links" :key="img" :src="img" :class="$style.preview" />
  </div>
</template>

<script setup lang="ts">
defineProps<{ links: string[] }>();
</script>

<style module>
.previews {
  overflow-x: auto;
  display: flex;
  gap: 4px;
  padding-bottom: 4px;
}

.preview {
  max-height: 128px;
  max-width: 128px;
  object-fit: contain;
  border: 1px solid transparent;
  background-color: #fafafa;
  padding: 4px;
  user-select: none;
  -webkit-user-drag: none;
}
</style>
