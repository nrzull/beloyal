<template>
  <Card with-border :with-shadow="false" class="v-space-12">
    <Space vertical>
      <div>Процент кэшбэка</div>
      <CashbackPercentInput v-model="model.cashbackPercent" size="medium" />
    </Space>

    <Space vertical>
      <div>Время и дата от</div>
      <NDatePicker type="datetime" placeholder="От" v-model:value="proxyFrom" />
    </Space>

    <Space vertical>
      <div>Время и дата до</div>
      <NDatePicker type="datetime" placeholder="До" v-model:value="proxyTo" />
    </Space>
  </Card>
</template>

<script setup lang="ts">
import { Card, Space } from "@beloyal/shared";
import type { DateTimePeriodVariant } from "@beloyal/shared";
import { formatISO, getUnixTime } from "date-fns";
import { NDatePicker } from "naive-ui";
import { computed } from "vue";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";

const model = defineModel<DateTimePeriodVariant>({ required: true });

const proxyFrom = computed({
  get: () => (model.value.from ? getUnixTime(model.value.from) * 1000 : null),
  set: (v) =>
    (model.value.from = formatISO(v!, { representation: "complete" })),
});

const proxyTo = computed({
  get: () => (model.value.to ? getUnixTime(model.value.to) * 1000 : null),
  set: (v) => (model.value.to = formatISO(v!, { representation: "complete" })),
});
</script>
