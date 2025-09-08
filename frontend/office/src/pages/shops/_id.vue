<template>
  <Page>
    <template #top>
      <NBreadcrumb>
        <NBreadcrumbItem @click="$router.push('/shops')">
          Торговые точки
        </NBreadcrumbItem>
        <NBreadcrumbItem>{{ shop?.name }}</NBreadcrumbItem>
      </NBreadcrumb>
    </template>

    <RouterView v-if="shop" :shop="shop" v-slot="{ Component }">
      <component :is="Component">
        <template #tabs>
          <NTabs :value="$route.path" @update:value="$router.push($event)">
            <NTab :name="`/shops/${shopId}`">Персонал</NTab>
            <NTab :name="`/shops/${shopId}/promotions`">Акции</NTab>
          </NTabs>
        </template>
      </component>
    </RouterView>
  </Page>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useRoute } from "vue-router";
import { apiShopsGet } from "~/api/shops";
import type { Shop } from "@beloyal/shared";
import Page from "~/components/Page.vue";
import { NBreadcrumb, NBreadcrumbItem, NTabs, NTab } from "naive-ui";

const route = useRoute();
const shopId = computed(() => route.params.id as string);
const shop = ref<Shop>();

apiShopsGet({ shopId: shopId.value }).then((v) => (shop.value = v.data));
</script>
