<template>
  <NSpace v-if="self" vertical :size="[0, 24]">
    <NSpace vertical>
      <div>Компания</div>

      <NSelect
        :key="selectedCompanyAccessId"
        size="large"
        :disabled="isLoading"
        placeholder="Выберите компанию"
        :render-label="(v: CompanyAccess) => v.company.tin"
        :options="self.companyAccesses || []"
        label-field="id"
        value-field="id"
        :value="selectedCompanyAccessId"
        @update:value="setCompanyAccess"
      >
        <template #arrow>
          <Icon variant="location" />
        </template>
      </NSelect>
    </NSpace>

    <NSpace vertical>
      <div>Торговая точка</div>
      <NSelect
        :key="selectedShopId"
        :disabled="isLoading || !selectedCompanyAccessId"
        size="large"
        placeholder="Выберите торговую точку"
        label-field="name"
        :options="self.shops || []"
        value-field="id"
        :value="selectedShopId"
        @update:value="setShopAccess"
      >
        <template #arrow>
          <Icon variant="location" />
        </template>
      </NSelect>
    </NSpace>
  </NSpace>
</template>

<script setup lang="ts">
import { NSelect, NSpace } from "naive-ui";
import { Icon, wait, type CompanyAccess } from "@beloyal/shared";
import { self, fetchSelf } from "~/composables/self";
import { computed } from "vue";
import { apiSelfSetCompanyAccess, apiSelfSetShopAccess } from "~/api/self";

const selectedShopId = computed(() => self.value?.shop?.id);
const selectedCompanyAccessId = computed(() => self.value?.companyAccess?.id);
const isLoading = computed(() => wait.some([setCompanyAccess, setShopAccess]));

const setCompanyAccess = async (companyAccessId: string) => {
  try {
    if (companyAccessId && selectedCompanyAccessId.value === companyAccessId) {
      return;
    }

    wait.start(setCompanyAccess);
    await apiSelfSetCompanyAccess({ companyAccessId });
    await fetchSelf();
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(setCompanyAccess);
  }
};

const setShopAccess = async (shopId: string) => {
  try {
    if (shopId && selectedShopId.value === shopId) {
      return;
    }

    wait.start(setShopAccess);

    await apiSelfSetShopAccess({
      shopId,
      companyAccessId: selectedCompanyAccessId.value!,
    });

    await fetchSelf();
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(setShopAccess);
  }
};
</script>
