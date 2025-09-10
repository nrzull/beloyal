<template>
  <div :class="$style.heading">
    <slot></slot>
  </div>
</template>

<style module>
.heading {
  font-size: 18px;
  display: inline-flex;
  align-items: center;
  gap: 8px;
}
</style>
