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

    <Space vertical v-if="withCountryCode">
      <div>Страна</div>
      <NSelect
        placeholder="Укажите страну"
        v-model:value="model.countryCode"
        :clearable="false"
        :disabled="loading"
        label-field="title"
        value-field="code"
        :options="countryCodes"
      />
    </Space>

    <Space vertical v-if="withTimezone">
      <div>Временная зона</div>
      <NSelect
        placeholder="Укажите временную зону"
        v-model:value="model.timeZone"
        :clearable="false"
        :disabled="!model.countryCode || loading"
        label-field="title"
        value-field="timeZone"
        :options="validTimeZones"
        filterable
      />
    </Space>
  </Space>
</template>

<script setup lang="ts">
import { Input, Space } from "@beloyal/shared";
import { NSelect } from "naive-ui";
import { computed } from "vue";
import type { CompanyFormModel } from "~/components/Company/types";
import { timeZones } from "~/utils/tz";

const { countryCodes = [], countryTimeZones = [] } = defineProps<{
  loading: boolean;
  withTimezone?: boolean;
  withCountryCode?: boolean;
  countryCodes?: Array<{ code: string; title: string }>;
  countryTimeZones?: Array<{ timeZone: string; title: string }>;
}>();

const model = defineModel<CompanyFormModel>({
  required: true,
});

const validTimeZones = computed(() =>
  countryTimeZones.filter((v) =>
    timeZones.some((vv) => vv.value === v.timeZone)
  )
);
</script>
