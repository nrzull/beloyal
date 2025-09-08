<template>
  <NSpace vertical>
    <NButton size="large" type="primary" block>
      <template #icon> <Icon variant="qr" /> </template>
      Сканировать
    </NButton>
    <NButton tertiary size="large" type="primary" block>
      <template #icon> <Icon variant="search" /> </template>
      Поиск по номеру телефона
    </NButton>
  </NSpace>
</template>

<script setup lang="ts">
import { Icon } from "@beloyal/shared";
import { NButton, NSpace } from "naive-ui";
</script>
