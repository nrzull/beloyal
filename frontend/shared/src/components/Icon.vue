<template>
  <div
    :class="[$style.icon, $style[`size-${size}`]]"
    v-html="icons[variant]"
  ></div>
</template>

<script setup lang="ts">
import user from "./Icon/assets/user.svg?raw";
import home from "./Icon/assets/home.svg?raw";
import qr from "./Icon/assets/qr.svg?raw";
import search from "./Icon/assets/search.svg?raw";
import location from "./Icon/assets/location.svg?raw";
import coins from "./Icon/assets/coins.svg?raw";
import rub from "./Icon/assets/rub.svg?raw";
import back from "./Icon/assets/back.svg?raw";

const icons = { user, home, qr, search, location, coins, rub, back } as const;

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
