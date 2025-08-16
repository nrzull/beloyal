<template>
  <div :class="$style.default">
    <div :class="$style.aside">
      <LayoutAside />
    </div>

    <div :class="$style.main">
      <RouterView />
    </div>
  </div>
</template>

<script setup lang="ts">
import LayoutAside from "~/components/LayoutAside.vue";
</script>

<style module>
.default {
  display: grid;
  grid-template-areas: "aside main";
  grid-template-columns: max-content 1fr;
  gap: 12px;
  width: inherit;
  height: inherit;
}

.aside {
  grid-area: aside;
  overflow-y: auto;
  overflow-x: clip;
  display: flex;
  justify-content: center;
  padding: 24px;
}

.main {
  grid-area: main;
  overflow-y: auto;
  overflow-x: clip;
  padding: 24px;
}
</style>
