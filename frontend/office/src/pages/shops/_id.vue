<template>
  <Page>
    <template #top>
      <NBreadcrumb>
        <NBreadcrumbItem @click="$router.push('/shops')">
          Торговые точки
        </NBreadcrumbItem>
        <NBreadcrumbItem @click="showShopModal = true">
          <Space align="center" :size="[8, 0]">
            <span>{{ shop?.name }}</span>
            <Icon :class="$style.edit" variant="edit" />
          </Space>
        </NBreadcrumbItem>
      </NBreadcrumb>
    </template>

    <RouterView v-if="shop" :shop="shop" v-slot="{ Component }">
      <component :is="Component" :loading="isLoading">
        <template #tabs>
          <NTabs :value="$route.path" @update:value="$router.push($event)">
            <NTab :name="`/shops/${shopId}`">Персонал</NTab>
            <NTab :name="`/shops/${shopId}/promotions`">Акции</NTab>
          </NTabs>
        </template>
      </component>
    </RouterView>
  </Page>

  <ShopModal
    v-if="showShopModal && shop"
    :shop="shop"
    :loading="isLoading"
    @submit="updateShop"
    @close="showShopModal = false"
  />
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useRoute } from "vue-router";
import { apiShopsGet, apiShopsUpdate } from "~/api/shops";
import type { Shop } from "@beloyal/shared";
import Page from "~/components/Page.vue";
import { NBreadcrumb, NBreadcrumbItem, NTabs, NTab } from "naive-ui";
import { Icon, Space, wait } from "@beloyal/shared";
import ShopModal from "~/components/Shops/ShopModal.vue";

const showShopModal = ref(false);
const route = useRoute();
const shopId = computed(() => route.params.id as string);
const shop = ref<Shop>();

const isLoading = computed(() => wait.some([fetchShop, updateShop]));

const updateShop = async (v: Shop) => {
  try {
    wait.start(updateShop);

    await apiShopsUpdate(v);
    await fetchShop();

    showShopModal.value = false;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(updateShop);
  }
};

const fetchShop = async () => {
  try {
    wait.start(fetchShop);
    const result = await apiShopsGet({ shopId: shopId.value });
    shop.value = result.data;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(fetchShop);
  }
};

fetchShop();
</script>

<style module>
.edit {
  cursor: pointer;
}

.edit:hover {
  color: var(--color-primary);
}
</style>
