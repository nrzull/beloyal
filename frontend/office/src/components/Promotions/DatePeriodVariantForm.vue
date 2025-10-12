<template>
  <Card with-border :with-shadow="false" class="v-space-12">
    <Space vertical>
      <div>Процент кэшбэка</div>
      <CashbackPercentInput v-model="model.cashbackPercent" size="medium" />
    </Space>

    <Space vertical>
      <div>Дата от</div>
      <NDatePicker placeholder="От" v-model:formatted-value="model.from" />
    </Space>

    <Space vertical>
      <div>Дата до</div>
      <NDatePicker placeholder="До" v-model:formatted-value="model.to" />
    </Space>
  </Card>
</template>

<script setup lang="ts">
import { Card, Space } from "@beloyal/shared";
import type { DatePeriodVariant } from "@beloyal/shared";
import { NDatePicker } from "naive-ui";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";

const model = defineModel<DatePeriodVariant>({ required: true });
</script>
