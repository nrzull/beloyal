<template>
  <Space vertical :size="[0, 12]">
    <Space vertical>
      <div>ИНН</div>
      <Input :disabled="loading" placeholder="ИНН" v-model:value="model.tin" />
    </Space>

    <Space vertical>
      <div>Название</div>
      <Input
        :disabled="loading"
        placeholder="Название"
        v-model:value="model.title"
      />
    </Space>

    <Space vertical v-if="withTimezone">
      <div>Временная зона</div>
      <NSelect
        placeholder="Укажите временную зону"
        v-model:value="model.timeZone"
        :clearable="false"
        :disabled="loading"
        label-field="label"
        value-field="value"
        :options="timeZones"
        filterable
      />
    </Space>
  </Space>
</template>

<script setup lang="ts">
import { Input, Space } from "@beloyal/shared";
import { timeZones } from "~/utils/tz";
import { NSelect } from "naive-ui";

defineProps<{ loading: boolean; withTimezone?: boolean }>();

const model = defineModel<{ tin: string; title: string; timeZone?: string }>({
  required: true,
});
</script>
