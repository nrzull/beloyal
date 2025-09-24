<template>
  <Space vertical>
    <Input placeholder="Фамилия" v-model:value="model.lastName" />
    <Input placeholder="Имя" v-model:value="model.firstName" />
    <Input
      placeholder="Отчество (опционально)"
      v-model:value="model.middleName"
    />
  </Space>
</template>

<script setup lang="ts">
import { Space, Input } from "@beloyal/shared";

const model = defineModel<{
  firstName?: string;
  lastName?: string;
  middleName?: string;
}>({ required: true });
</script>
