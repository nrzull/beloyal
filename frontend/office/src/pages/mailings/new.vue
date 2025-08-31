<template>
  <Page>
    <template #top>
      <NBreadcrumb>
        <NBreadcrumbItem @click="$router.push('/mailings')">
          Рассылки
        </NBreadcrumbItem>
        <NBreadcrumbItem>Новая рассылка</NBreadcrumbItem>
      </NBreadcrumb>
    </template>

    <Card class="v-space-24">
      <div :class="$style.switch">
        <div>Торговые точки</div>
        <NSwitch
          v-model:value="withShops"
          :checked-value="false"
          :unchecked-value="true"
          :disabled="isLoading"
        >
        </NSwitch>
        <div>Все клиенты</div>
      </div>
      <NSelect
        v-if="withShops"
        placeholder="Выберите торговые точки"
        v-model:value="selectedShopIds"
        multiple
        label-field="name"
        value-field="id"
        :options="shops"
      />
      <NInput type="textarea" placeholder="Текст" v-model:value="text" />
      <NButton :disabled="isLoading" type="primary" @click="createMailing">
        Создать рассылку
      </NButton>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import {
  NBreadcrumb,
  NBreadcrumbItem,
  NButton,
  NInput,
  NSelect,
  NSwitch,
} from "naive-ui";
import Card from "~/components/Card/Card.vue";
import { computed, ref } from "vue";
import { wait } from "~/composables/wait";

import { apiShopsGetAll } from "~/api/shops";
import type { Shop } from "~/api/shops/types";

const withShops = ref(true);
const shops = ref<Shop[]>([]);
const selectedShopIds = ref<string[]>([]);
const text = ref<string>();

const isLoading = computed(() => wait.some([createMailing]));

apiShopsGetAll().then((v) => (shops.value = v.data));

const createMailing = async () => {
  if (withShops.value && !selectedShopIds.value.length) {
    return;
  }

  if (!text.value) {
    return;
  }

  try {
    wait.start(createMailing);
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(createMailing);
  }
};
</script>

<style module>
.switch {
  display: flex;
  align-items: center;
  gap: 8px;
}
</style>
