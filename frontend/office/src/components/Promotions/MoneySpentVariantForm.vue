<template>
  <Card with-border :with-shadow="false" class="v-space-24">
    <Space vertical>
      <div>Процент кэшбэка</div>
      <CashbackPercentInput v-model="model.cashbackPercent" size="medium" />
    </Space>

    <Space vertical>
      <div>Потрачено</div>
      <NInputNumber v-model:value="model.amount" :min="0" :precision="0" />
    </Space>

    <Space vertical :size="[0, 12]">
      <Space align="center" justify="space-between">
        <div>Период</div>

        <Button
          v-if="!model.period"
          type="primary"
          size="small"
          @click="model.period = serialize({ days: 30, hours: 0, minutes: 0 })"
        >
          Добавить
        </Button>
        <Button
          v-else
          ghost
          type="error"
          size="small"
          @click="model.period = undefined"
          >Удалить</Button
        >
      </Space>

      <DurationForm v-if="model.period" v-model="model.period" />
    </Space>
  </Card>
</template>

<script setup lang="ts">
import { Card, Space, Button } from "@beloyal/shared";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";
import { NInputNumber } from "naive-ui";
import DurationForm from "~/components/DurationForm.vue";
import type { MoneySpentVariant } from "@beloyal/shared";
import { serialize } from "tinyduration";

const model = defineModel<MoneySpentVariant>({ required: true });
</script>
