<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading> Компания </Heading>
    </template>

    <Card v-if="model" class="v-space-24">
      <CompanyForm
        :loading="isLoading"
        v-model="model"
        with-timezone
        :country-time-zones="transformCountryTimeZones(countryZones)"
      />
      <Button :disabled="isLoading" type="primary" @click="updateCompany">
        Сохранить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, useToast, wait, waitable } from "@beloyal/shared";
import { self } from "~/composables/self";
import { Card, Button } from "@beloyal/shared";
import { computed, ref } from "vue";
import { cloneDeep } from "es-toolkit";
import CompanyForm from "~/components/Company/CompanyForm.vue";
import { apiCompanyUpdate } from "~/api/company";
import { fetchSelf } from "~/composables/self";
import { apiCountriesGetTimeZones } from "~/api/countries";
import type { CompanyFormModel } from "~/components/Company/types";
import { transformCountryTimeZones } from "~/components/Countries/utils";

const model = ref<CompanyFormModel>();
const { showOk, showError } = useToast();
const isLoading = computed(() =>
  wait.some([apiCountriesGetTimeZones, fetchSelf, updateCompany])
);
const countryZones = ref<string[]>([]);

const updateCompany = async () => {
  if (
    !model.value ||
    !model.value.tin ||
    !model.value.title ||
    !model.value.timeZone
  ) {
    return;
  }

  try {
    wait.start(updateCompany);
    await apiCompanyUpdate({ ...model.value, timeZone: model.value.timeZone! });
    await fetchCompany();
    showOk();
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(updateCompany);
  }
};

const fetchCompany = async () => {
  await waitable(fetchSelf)();

  model.value = cloneDeep(self.value?.companyAccess?.company!);

  const result = await waitable(apiCountriesGetTimeZones)({
    countryCode: model.value.countryCode,
  });

  countryZones.value = result.data;
};

fetchCompany();
</script>
