<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Новая торговая точка</Heading>
    </template>

    <Card class="v-space-24">
      <ShopForm
        v-model="model"
        :loading="isLoading"
        :country-time-zones="transformCountryTimeZones(countryZones)"
      />

      <Button :disabled="isLoading" type="primary" @click="createShop">
        Добавить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, Card, Button, useToast, waitable } from "@beloyal/shared";
import { computed, ref } from "vue";
import { wait } from "@beloyal/shared";
import { useRouter } from "vue-router";
import { apiShopsCreate } from "~/api/shops";
import ShopForm from "~/components/Shops/ShopForm.vue";
import type { PartialShop } from "~/components/Shops/utils";
import { transformCountryTimeZones } from "~/components/Countries/utils";
import { apiCountriesGetTimeZones } from "~/api/countries";
import { self } from "~/composables/self";

const isLoading = computed(() =>
  wait.some([apiCountriesGetTimeZones, createShop])
);
const router = useRouter();
const { showOk, showError } = useToast();
const model = ref<PartialShop>({ name: "", timeZone: undefined });

const createShop = async () => {
  if (!model.value.name) {
    return;
  }

  try {
    wait.start(createShop);
    await apiShopsCreate(model.value);
    showOk();
    router.push("/shops");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(createShop);
  }
};

const countryZones = ref<string[]>([]);

waitable(apiCountriesGetTimeZones)({
  countryCode: self.value?.companyAccess!.company.countryCode!,
}).then((v) => (countryZones.value = v.data));
</script>
