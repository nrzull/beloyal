<template>
  <Space vertical :size="[0, 16]">
    <Space vertical>
      <div>Дней</div>
      <NInputNumber
        size="large"
        :min="0"
        :precision="0"
        placeholder="Укажите число"
        :value="proxyModel.days"
        @update:value="proxyModel = { ...proxyModel, days: $event! }"
      />
    </Space>

    <Space vertical>
      <div>Часов</div>
      <NInputNumber
        size="large"
        :min="0"
        :max="23"
        :precision="0"
        placeholder="Укажите число"
        :value="proxyModel.hours"
        @update:value="proxyModel = { ...proxyModel, hours: $event! }"
      />
    </Space>

    <Space vertical>
      <div>Минут</div>
      <NInputNumber
        size="large"
        :min="0"
        :max="59"
        :precision="0"
        placeholder="Укажите число"
        :value="proxyModel.minutes"
        @update:value="proxyModel = { ...proxyModel, minutes: $event! }"
      />
    </Space>
  </Space>
</template>

<script setup lang="ts">
import { NInputNumber } from "naive-ui";
import { Space } from "@beloyal/shared";
import { parse, serialize } from "tinyduration";
import { computed } from "vue";

const model = defineModel<string>({ required: true });

const proxyModel = computed({
  get: () => parse(model.value),
  set: (v) => (model.value = serialize(v!)),
});
</script>
