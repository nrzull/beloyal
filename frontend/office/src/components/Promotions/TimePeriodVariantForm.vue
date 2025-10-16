<template>
  <Card with-border :with-shadow="false" class="v-space-12">
    <Space vertical>
      <div>Процент кэшбэка</div>
      <CashbackPercentInput v-model="model.cashbackPercent" size="medium" />
    </Space>

    <Space vertical>
      <div>Время от</div>
      <NTimePicker placeholder="От" v-model:value="proxyFrom" />
    </Space>

    <Space vertical>
      <div>Время до</div>
      <NTimePicker placeholder="До" v-model:value="proxyTo" />
    </Space>
  </Card>
</template>

<script setup lang="ts">
import { Card, Space } from "@beloyal/shared";
import type { TimePeriodVariant } from "@beloyal/shared";
import { computed } from "vue";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";
import {
  format,
  getHours,
  getMinutes,
  getSeconds,
  parseISO,
  set as setTime,
} from "date-fns";
import { NTimePicker } from "naive-ui";

const model = defineModel<TimePeriodVariant>({ required: true });

const toDateTimeString = (v: string) => `1970-01-01T${v}`;

// const toTimeObj = (v: Date): TimeObj => {
//   const h = getHours(v);
//   const m = getMinutes(v);
//   const s = getSeconds(v);

//   return { hours: h, minutes: m, seconds: s };
// };

// const fromTimeModel = (v: TimeObj): Date => setTime(new Date(), v);

const proxyFrom = computed({
  get: () => {
    if (!model.value.from) {
      return undefined;
    }

    return Date.parse(toDateTimeString(model.value.from));
  },
  set: (v) => {
    if (!v) {
      model.value.from = null;
      return;
    }

    model.value.from = format(new Date(v), "HH:mm:SS");
  },
});

const proxyTo = computed({
  get: () => {
    if (!model.value.to) {
      return undefined;
    }

    return Date.parse(toDateTimeString(model.value.to));
  },
  set: (v) => {
    if (!v) {
      model.value.to = null;
      return;
    }

    model.value.to = format(new Date(v), "HH:mm:SS");
  },
});
</script>
