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
import history from "./Icon/assets/history.svg?raw";
import menu from "./Icon/assets/menu.svg?raw";
import chevronRight from "./Icon/assets/chevronRight.svg?raw";
import percent from "./Icon/assets/percent.svg?raw";
import telegram from "./Icon/assets/telegram.svg?raw";

const icons = {
  user,
  home,
  qr,
  search,
  location,
  coins,
  rub,
  back,
  history,
  menu,
  chevronRight,
  percent,
  telegram,
} as const;

export type IconVariant = keyof typeof icons;

const { size = "s" } = defineProps<{
  variant: IconVariant;
  size?: "s" | "m";
}>();
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
