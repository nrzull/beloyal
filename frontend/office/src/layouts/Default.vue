<template>
  <div :class="$style.default">
    <div :class="$style.aside"></div>

    <div :class="$style.main">
      <RouterView />
    </div>
  </div>
</template>

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
