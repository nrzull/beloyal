<template>
  <Space v-if="self" vertical :size="[0, 24]">
    <Space vertical>
      <div>Компания</div>

      <NSelect
        :key="selectedCompanyAccessId"
        size="large"
        :disabled="isLoading"
        placeholder="Выберите компанию"
        :render-label="(v: CompanyAccess) => v.company.title"
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
    </Space>

    <Space vertical>
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
    </Space>

    <Button
      size="large"
      type="primary"
      ghost
      block
      @click="$router.push('/auth')"
    >
      Сменить учетную запись
    </Button>
  </Space>
</template>

<script setup lang="ts">
import { NSelect } from "naive-ui";
import { Icon, Space, Button, wait, type CompanyAccess } from "@beloyal/shared";
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
