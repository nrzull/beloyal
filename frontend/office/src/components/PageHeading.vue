<template>
  <div :class="$style.pageHeading">
    <slot></slot>
  </div>
</template>

<style module>
.pageHeading {
  font-size: 18px;
}
</style>
