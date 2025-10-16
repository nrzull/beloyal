<template>
  <Card with-border :with-shadow="false" class="v-space-12">
    <Space vertical>
      <div>Процент кэшбэка</div>
      <CashbackPercentInput v-model="model.cashbackPercent" size="medium" />
    </Space>

    <Space vertical>
      <div>Дата от</div>
      <NDatePicker placeholder="От" v-model:value="proxyFrom" />
    </Space>

    <Space vertical>
      <div>Дата до</div>
      <NDatePicker placeholder="До" v-model:value="proxyTo" />
    </Space>
  </Card>
</template>

<script setup lang="ts">
import { Card, Space } from "@beloyal/shared";
import type { DatePeriodVariant } from "@beloyal/shared";
import { NDatePicker } from "naive-ui";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";
import { formatISO, getUnixTime } from "date-fns";
import { computed } from "vue";

const model = defineModel<DatePeriodVariant>({ required: true });

const proxyFrom = computed({
  get: () => (model.value.from ? getUnixTime(model.value.from) * 1000 : null),
  set: (v) => (model.value.from = formatISO(v!, { representation: "date" })),
});

const proxyTo = computed({
  get: () => (model.value.to ? getUnixTime(model.value.to) * 1000 : null),
  set: (v) => (model.value.to = formatISO(v!, { representation: "date" })),
});
</script>
