<template>
  <Card with-border :with-shadow="false" class="v-space-12">
    <Space vertical>
      <div>Процент кэшбэка</div>
      <CashbackPercentInput v-model="model.cashbackPercent" size="medium" />
    </Space>

    <Space vertical>
      <div>Операция по счету</div>
      <NInputNumber
        placeholder="Количество"
        v-model:value="model.count"
        :min="1"
        :precision="0"
      />
    </Space>
  </Card>
</template>

<script setup lang="ts">
import { Card, Space } from "@beloyal/shared";
import type { ExactCountVariant } from "@beloyal/shared";
import { NInputNumber } from "naive-ui";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";

const model = defineModel<ExactCountVariant>({ required: true });
</script>
