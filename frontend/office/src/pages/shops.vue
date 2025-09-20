<template>
  <Page>
    <template #top>
      <PageHeading>Торговые точки</PageHeading>
      <Button type="primary" size="medium" @click="$router.push('/shops/new')">
        Добавить точку
      </Button>
    </template>

    <Table
      :data="shops"
      :columns="[
        { key: 'name', title: 'Название' },
        { key: 'createdAt', title: 'Создано' },
      ]"
      :route-fn="(v) => `/shops/${v.id}`"
    >
      <template #createdAt="{ item }">
        {{ formatDate(item.createdAt) }}
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import { ref } from "vue";
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Table from "~/components/Table/Table.vue";
import { formatDate } from "~/utils/date";
import type { Shop } from "@beloyal/shared";
import { Button } from "@beloyal/shared";
import { apiShopsGetAll } from "~/api/shops";

const shops = ref<Shop[]>([]);

apiShopsGetAll().then((v) => (shops.value = v.data));
</script>
