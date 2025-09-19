<template>
  <div class="v-space-24">
    <div :class="$style.top">
      <slot name="tabs"></slot>
      <NButton type="primary" @click="addShopAccessModal = true">
        Назначить сотрудника
      </NButton>
    </div>

    <Table
      :data="shopAccesses"
      :columns="[
        { key: 'phone', title: 'Номер телефона' },
        { key: 'createdAt', title: 'Создано' },
      ]"
    >
      <template #phone="{ item }">
        {{ maskPhone(item.phone) }}
      </template>

      <template #createdAt="{ item }">
        {{ formatDate(item.createdAt) }}
      </template>
    </Table>
  </div>

  <NModal :class="$style.modal" v-model:show="addShopAccessModal" preset="card">
    <template #header>Назначить сотрудника</template>

    <Table
      clickable
      :card="{ withPadding: false, withShadow: false }"
      :data="availableCompanyAccesses"
      :selected-fn="
        (v) => shopAccesses.some((vv) => vv.companyAccessId === v.id)
      "
      @row="addShopAccess"
      :columns="[
        { key: 'phone', title: 'Номер телефона' },
        { key: 'role', title: 'Роль' },
      ]"
    >
      <template #phone="{ item }">
        {{ maskPhone(item.user?.phone) }}
      </template>

      <template #role="{ item }">
        {{ item.role?.name }}
      </template>
    </Table>
  </NModal>
</template>

<script setup lang="ts">
import { NButton, NModal } from "naive-ui";
import { computed, ref } from "vue";
import { apiCompanyAccessesGetAll } from "~/api/companyAccesses";
import type { CompanyAccess } from "@beloyal/shared";
import type { ShopAccess } from "@beloyal/shared";
import { apiShopsGetAccesses, apiShopsCreateAccess } from "~/api/shops";
import type { Shop } from "@beloyal/shared";
import Table from "~/components/Table/Table.vue";
import { wait } from "@beloyal/shared";
import { formatDate } from "~/utils/date";
import { maskPhone } from "@beloyal/shared";

const { shop } = defineProps<{ shop: Shop }>();

const addShopAccessModal = ref(false);
const shopAccesses = ref<ShopAccess[]>([]);
const availableCompanyAccesses = ref<CompanyAccess[]>([]);
const isLoading = computed(() => wait.some([addShopAccess, fetchShopAccesses]));

const addShopAccess = async (v: CompanyAccess) => {
  if (
    isLoading.value ||
    shopAccesses.value.some((vv) => vv.companyAccessId === v.id)
  ) {
    return;
  }

  try {
    wait.start(addShopAccess);

    await apiShopsCreateAccess({ shopId: shop.id, companyAccessId: v.id });
    await fetchShopAccesses();
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(addShopAccess);
  }
};

const fetchShopAccesses = async () => {
  try {
    wait.start(fetchShopAccesses);

    const result = await apiShopsGetAccesses({ shopId: shop.id });
    shopAccesses.value = result.data;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(fetchShopAccesses);
  }
};

apiCompanyAccessesGetAll().then(
  (v) => (availableCompanyAccesses.value = v.data)
);

fetchShopAccesses();
</script>

<style module>
.modal {
  max-width: 720px;
}

.top {
  display: flex;
}
</style>
