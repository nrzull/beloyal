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
import { NTimePicker } from "naive-ui";
import { computed } from "vue";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";

const model = defineModel<TimePeriodVariant>({ required: true });

const proxyFrom = computed({
  get: () => (model.value.from === null ? undefined : model.value.from),
  set: (v) => (model.value.from = v || null),
});

const proxyTo = computed({
  get: () => (model.value.to === null ? undefined : model.value.to),
  set: (v) => (model.value.to = v || null),
});
</script>
