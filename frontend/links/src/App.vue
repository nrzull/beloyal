<template>
  <div :class="$style.root">
    <Space vertical>
      <a
        v-for="link in links"
        :key="link.url"
        :href="link.url"
        rel="nofollow"
        taget="_blank"
        :class="[$style.card, $style[link.type]]"
      >
        <Icon size="m" :class="$style.icon" :variant="link.type" />
        <template v-if="link.type === 'telegram'">Telegram</template>
      </a>
    </Space>
  </div>
</template>

<script setup lang="ts">
import { Space, Card, Icon, type CompanyLink } from "@beloyal/shared";
const url = new URL(location.href);
const payload = url.searchParams.get("payload");

const links: CompanyLink[] = JSON.parse(
  decodeURIComponent(atob(decodeURIComponent(payload!)))
);
</script>

<style module>
.root {
  padding: 24px;
  margin: 0 auto;
  display: flex;
  flex-flow: column;
  gap: 4px;
  max-width: 375px;
  width: 100%;
}

.root a {
  text-decoration: none;
  color: initial;
}

.card {
  padding: 16px;
  width: 100%;
  border-radius: 100px;
  display: flex;
  align-items: center;
  font-size: 18px;
  font-weight: 500;
  gap: 8px;
}

.card.telegram {
  color: #fff;
  background-color: #54a9eb;
}
</style>
