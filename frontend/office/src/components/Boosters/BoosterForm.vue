<template>
  <Space vertical>
    <CashbackPercentInput v-model="model.cashbackPercent" />

    <NDatePicker
      v-model:value="proxyExpiredAt"
      placeholder="Укажите срок доступа"
    />
  </Space>
</template>

<script setup lang="ts">
import { Space } from "@beloyal/shared";
import { formatISO, startOfDay } from "date-fns";
import { NDatePicker } from "naive-ui";
import { computed } from "vue";
import CashbackPercentInput from "~/components/CashbackPercentInput.vue";

const model = defineModel<{ cashbackPercent: number; expiredAt: string }>({
  required: true,
});

const proxyExpiredAt = computed({
  get: () => Date.parse(model.value.expiredAt),
  set: (v) => (model.value.expiredAt = formatISO(startOfDay(v || new Date()))),
});
</script>
