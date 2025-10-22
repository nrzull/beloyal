<template>
  <Card with-border :with-shadow="false" class="v-space-24">
    <Space vertical>
      <div>Процент кэшбэка</div>
      <CashbackPercentInput v-model="model.cashbackPercent" size="medium" />
    </Space>

    <Space vertical :size="[0, 12]">
      <div>Период неактивности</div>
      <DurationForm v-model="model.period" />
    </Space>
  </Card>
</template>

<script setup lang="ts">
import { Card, Space } from "@beloyal/shared";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";
import type { OperationsInactivityVariant } from "@beloyal/shared";
import DurationForm from "~/components/DurationForm.vue";

const model = defineModel<OperationsInactivityVariant>({ required: true });
</script>
