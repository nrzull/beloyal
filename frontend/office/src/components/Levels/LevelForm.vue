<template>
  <Space vertical :size="[0, 12]">
    <Space vertical :size="[0, 8]">
      <div>Очки</div>
      <NInputNumber
        :disabled="loading"
        :min="1"
        :precision="0"
        placeholder="Укажите верхнюю грань"
        v-model:value="model.pointsCapacity"
      />
    </Space>

    <Space vertical :size="[0, 8]">
      <div>Кэшбэк</div>
      <CashbackPercentInput
        :disabled="loading"
        size="medium"
        v-model="model.cashbackPercent"
      />
    </Space>

    <Space vertical :size="[0, 8]">
      <div>Описание (опционально)</div>
      <Input
        :disabled="loading"
        type="textarea"
        placeholder="Укажите описание"
        v-model:value="model.description"
      />
    </Space>
  </Space>
</template>

<script setup lang="ts">
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";
import { NInputNumber } from "naive-ui";
import { Input, type Level } from "@beloyal/shared";
import { Space } from "@beloyal/shared";

defineProps<{ loading: boolean }>();

const model = defineModel<
  Pick<Level, "cashbackPercent" | "description" | "pointsCapacity">
>({ required: true });
</script>
