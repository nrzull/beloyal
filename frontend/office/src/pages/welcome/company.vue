<template>
  <div :class="$style.company">
    <Card :class="$style.form">
      <CompanyForm
        :loading="isLoading"
        v-model="model"
        with-country-code
        with-timezone
        :country-time-zones="transformCountryTimeZones(countryTimeZones)"
        :country-codes="transformCountryCodes(countryCodes)"
      />
      <Button type="primary" :disabled="isLoading" @click="createCompany">
        Создать компанию
      </Button>
      <Button
        :disabled="isLoading"
        type="primary"
        ghost
        @click="$router.push('/welcome')"
      >
        Назад
      </Button>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { Button, Card, useToast } from "@beloyal/shared";
import { computed, ref, watch } from "vue";
import { apiSelfCreateCompany } from "~/api/self";
import { wait, waitable } from "@beloyal/shared";
import { useRouter } from "vue-router";
import CompanyForm from "~/components/Company/CompanyForm.vue";
import {
  apiCountriesGetAll,
  apiCountriesGetLangs,
  apiCountriesGetTimeZones,
} from "~/api/countries";
import type { CompanyFormModel } from "~/components/Company/types";
import {
  transformCountryCodes,
  transformCountryTimeZones,
} from "~/components/Countries/utils";

const model = ref<CompanyFormModel>({
  tin: "",
  title: "",
  countryCode: undefined as any,
  countryLangs: [],
});

watch(
  () => model.value.countryCode,
  (v) => {
    waitable(apiCountriesGetLangs)({ countryCode: v }).then(
      (v) => (model.value.countryLangs = v.data)
    );

    waitable(apiCountriesGetTimeZones)({ countryCode: v }).then(
      (v) => (countryTimeZones.value = v.data)
    );
  }
);

const countryCodes = ref<string[]>([]);
const countryTimeZones = ref<string[]>([]);

const router = useRouter();
const isLoading = computed(() =>
  wait.some([
    apiCountriesGetLangs,
    apiCountriesGetAll,
    apiCountriesGetTimeZones,
    createCompany,
  ])
);
const { showOk, showError } = useToast();

const createCompany = async () => {
  if (!model.value.tin || !model.value.title) {
    return;
  }

  try {
    wait.start(createCompany);

    await apiSelfCreateCompany({
      ...model.value,
      cashbackPercent: 10,
      cashbackThreshold: 25,
      bonusMultiplier: 10,
      bonusLifetimeInDays: 180,
      levelLifetimeInDays: 14,
      timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone,
    });

    showOk();
    router.push("/");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(createCompany);
  }
};

waitable(apiCountriesGetAll)().then((v) => (countryCodes.value = v.data));
</script>

<style module>
.company {
  display: flex;
  justify-content: center;
  align-items: center;
  width: inherit;
  height: inherit;
}

.form {
  width: 375px;
  display: grid;
  gap: 12px;
}
</style>
