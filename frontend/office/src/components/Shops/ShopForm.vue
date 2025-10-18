<template>
  <Space vertical :size="[0, 12]">
    <Space vertical>
      <div>Название</div>
      <Input
        :disabled="loading"
        placeholder="Укажите название"
        v-model:value="model.name"
      />
    </Space>

    <Space vertical>
      <div>
        Временная зона
        <Subheading type="notice">
          (если оставить пустым, то будет использоваться временная зона
          компании)
        </Subheading>
      </div>

      <NSelect
        placeholder="Укажите временную зону"
        v-model:value="model.timeZone"
        clearable
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
import { Space, Subheading, Input } from "@beloyal/shared";
import { NSelect } from "naive-ui";
import { timeZones } from "~/utils/tz";

defineProps<{ loading: boolean }>();

const model = defineModel<{ name: string; timeZone?: string }>({
  required: true,
});
</script>
