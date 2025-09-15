<template>
  <div :class="$style.mobileLayout">
    <div :class="$style.content">
      <RouterView />
    </div>

    <slot name="footer" v-bind="{ footerClass: $style.footer }"></slot>
  </div>
</template>

<script setup lang="ts"></script>

<style module>
.mobileLayout {
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
