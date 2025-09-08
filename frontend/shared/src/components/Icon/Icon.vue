<template>
  <div
    :class="[$style.icon, $style[`size-${size}`]]"
    v-html="icons[variant]"
  ></div>
</template>

<script setup lang="ts">
import user from "./assets/user.svg?raw";
import home from "./assets/home.svg?raw";
import qr from "./assets/qr.svg?raw";
import search from "./assets/search.svg?raw";

const icons = { user, home, qr, search } as const;

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
