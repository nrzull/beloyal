<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading> Компания </Heading>
    </template>

    <Card v-if="model" class="v-space-24">
      <CompanyForm :loading="isLoading" v-model="model" with-timezone />
      <Button :disabled="isLoading" type="primary" @click="updateCompany">
        Сохранить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, wait, waitable } from "@beloyal/shared";
import { self } from "~/composables/self";
import { Card, Button } from "@beloyal/shared";
import { computed, ref } from "vue";
import { cloneDeep } from "es-toolkit";
import CompanyForm from "~/components/Company/CompanyForm.vue";
import { apiCompanyUpdate } from "~/api/company";
import { fetchSelf } from "~/composables/self";

const model = ref();

const isLoading = computed(() => wait.some([fetchSelf, updateCompany]));

const updateCompany = async () => {
  try {
    wait.start(updateCompany);
    await apiCompanyUpdate(model.value);
    await fetchCompany();
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(updateCompany);
  }
};

const fetchCompany = async () => {
  await waitable(fetchSelf)();
  model.value = cloneDeep(self.value?.companyAccess?.company!);
};

fetchCompany();
</script>
