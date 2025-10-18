<template>
  <NInputNumber
    size="large"
    placeholder="Укажите процент"
    :min="1"
    :max="100"
    :precision="0"
    v-model:value="model"
  >
    <template #prefix>
      <Icon :style="{ color: 'var(--color-grey-500)' }" variant="percent" />
    </template>
  </NInputNumber>
</template>

<script setup lang="ts">
import { Icon } from "@beloyal/shared";
import { NInputNumber } from "naive-ui";

const model = defineModel<number>();
</script>
