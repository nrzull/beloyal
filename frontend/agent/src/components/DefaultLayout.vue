<template>
  <div :class="$style.defaultLayout">
    <div :class="$style.content">
      <RouterView />
    </div>
    <ViewFooter :class="$style.footer" />
  </div>
</template>

<script setup lang="ts">
import ViewFooter from "~/components/ViewFooter.vue";
</script>

<style module>
.defaultLayout {
  display: grid;
  grid-template-areas: "content" "footer";
  grid-template-rows: 1fr 48px;
  height: inherit;
  max-width: 768px;
  width: 100%;
  margin: auto;
}

.content {
  grid-area: content;
  overflow: hidden;
  word-break: break-all;
  overflow-y: auto;
  padding: 12px;
}

.footer {
  grid-area: footer;
}
</style>
