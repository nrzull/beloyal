<template>
  <div :class="$style.defaultLayout">
    <div :class="$style.header">header</div>
    <div :class="$style.content">
      <RouterView />
    </div>
    <div :class="$style.footer">footer</div>
  </div>
</template>

<style module>
.defaultLayout {
  display: grid;
  grid-template-areas: "header" "content" "footer";
  grid-template-rows: 48px 1fr 48px;
  height: inherit;
}

.header {
  grid-area: header;
  display: flex;
  align-items: center;
  padding: 12px;
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
  padding: 12px;
}
</style>
