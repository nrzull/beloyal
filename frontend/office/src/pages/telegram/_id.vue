<template>
  <Page>
    <template #top>
      <Heading>Telegram</Heading>
    </template>

    <Card>
      <NThing>
        <template #avatar>
          <component
            :is="bot.photo ? 'img' : 'div'"
            :src="bot.photo"
            :class="$style.photo"
          />
        </template>

        <template #header>
          <Heading>{{ bot.name }}</Heading>
        </template>

        <template #header-extra>
          <NTag type="success">Подключён</NTag>
        </template>

        <template #description>
          <Space vertical>
            <a :class="$style.username" :href="`https://t.me/${bot.username}`">
              @{{ bot.username }}
            </a>
            <div>{{ bot.shortDescription }}</div>
            <div>{{ bot.description }}</div>
          </Space>
        </template>
      </NThing>

      <Space vertical>
        <Space align="center"> </Space>
      </Space>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import type { TgBot } from "@beloyal/shared";
import { Card, Heading, Space } from "@beloyal/shared";
import { NTag, NThing } from "naive-ui";
import Page from "~/components/Page.vue";

defineProps<{ bot: TgBot }>();
</script>

<style module>
.username {
  color: var(--color-primary);
  text-decoration: none;
}

.photo {
  border-radius: 100px;
  width: 96px;
  height: 96px;
  background-color: #fafafa;
}
</style>
