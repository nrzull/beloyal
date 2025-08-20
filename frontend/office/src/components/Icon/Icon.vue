<template>
  <div
    :class="[$style.icon, $style[`size-${size}`]]"
    v-html="icons[variant]"
  ></div>
</template>

<script setup lang="ts">
import x from "~/components/Icon/assets/x.svg?raw";
import errorCircleFilled from "~/components/Icon/assets/error-circle-filled.svg?raw";

const icons = {
  x,
  errorCircleFilled,
} as const;

export type Variant = keyof typeof icons;

const { size = "s" } = defineProps<{ variant: Variant; size?: "s" | "m" }>();
</script>

<style module>
.icon {
  line-height: 0;
}

.size-s {
  min-width: 19px;
  max-width: 19px;
}

.size-m {
  min-width: 24px;
  max-width: 24px;
}
</style>
