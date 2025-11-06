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

    <NSelect
      v-model:value="model.countryLang"
      :placeholder="t('userCredentialsForm.countryLang')"
      :options="adaptedKnownLocales"
      value-field="value"
      label-field="title"
    />
  </Space>
</template>

<script setup lang="ts">
import { Space, Input } from "@beloyal/shared";
import { useI18n } from "vue-i18n";
import type { CountryMessages } from "~/locales/types";
import { knownLocales, type KnownLocale } from "~/locales/utils";
import { NSelect } from "naive-ui";
import { computed } from "vue";
import { translateLocales } from "~/locales/utils";

const model = defineModel<{
  firstName?: string;
  lastName?: string;
  middleName?: string;
  countryLang?: string;
}>({ required: true });

const { t } = useI18n<{ message: CountryMessages }>();

const adaptedKnownLocales = computed(() =>
  Object.keys(knownLocales).map((k) => ({
    value: k,
    title: translateLocales[k as KnownLocale],
  }))
);
</script>
