<template>
  <Space vertical>
    <Input
      :placeholder="t('userCredentialsForm.lastName')"
      v-model:value="model.lastName"
    />

    <Input
      :placeholder="t('userCredentialsForm.firstName')"
      v-model:value="model.firstName"
    />

    <Input
      :placeholder="t('userCredentialsForm.middleName')"
      v-model:value="model.middleName"
    />
  </Space>
</template>

<script setup lang="ts">
import { Space, Input } from "@beloyal/shared";
import { useI18n } from "vue-i18n";
import type { CountryMessages } from "~/locales/types";

const model = defineModel<{
  firstName?: string;
  lastName?: string;
  middleName?: string;
}>({ required: true });

const { t } = useI18n<{ message: CountryMessages }>();
</script>
