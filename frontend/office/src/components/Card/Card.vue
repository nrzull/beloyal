<template>
  <div :class="$style.card">
    <slot></slot>
  </div>
</template>

<style module>
.card {
  background-color: var(--color-white);
  padding: 32px 24px;
  border-radius: 8px;
  box-shadow: var(--box-shadow-card);
}
</style>
