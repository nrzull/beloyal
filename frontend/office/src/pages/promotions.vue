<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Акции</Heading>
      <Button
        type="primary"
        size="medium"
        @click="$router.push('/promotions/new')"
      >
        Создать акцию
      </Button>
    </template>

    <Table
      :data="promotions"
      :columns="[
        { key: 'title', title: 'Название' },
        { key: 'variant', title: 'Тип' },
        { key: 'cashbackPercent', title: 'Кэшбэк' },
        { key: 'createdAt', title: 'Дата создания' },
      ]"
      :route-fn="(v) => `/promotions/${v.id}`"
    >
      <template #variant="{ item }">
        {{ translatePromotionVariantType[item.variant.$type] }}
      </template>

      <template #cashbackPercent="{ item }">
        {{ item.variant.cashbackPercent }}%
      </template>

      <template #createdAt="{ item }">
        {{ formatDate(item.createdAt) }}
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import {
  Heading,
  Button,
  Table,
  type Promotion,
  formatDate,
  wait,
  waitable,
} from "@beloyal/shared";
import { computed, ref } from "vue";
import { apiPromotionsGetAll } from "~/api/promotions";
import { translatePromotionVariantType } from "~/components/Promotions/utils";

const promotions = ref<Promotion[]>([]);

const isLoading = computed(() => wait.some([apiPromotionsGetAll]));

waitable(apiPromotionsGetAll)().then((v) => (promotions.value = v.data));
</script>
