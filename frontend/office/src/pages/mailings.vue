<template>
  <Page>
    <template #top>
      <PageHeading>Рассылки</PageHeading>

      <NButton
        type="primary"
        size="medium"
        @click="$router.push('/mailings/new')"
      >
        Новая рассылка
      </NButton>
    </template>

    <Table
      :data="mailings"
      :columns="[
        { key: 'createdAt', title: 'Создано' },
        { key: 'done', title: 'Получено' },
        { key: 'cancelled', title: 'Отменено' },
        { key: 'total', title: 'Всего' },
      ]"
      :route-fn="(v) => `/company-roles/${v.id}`"
    >
      <template #createdAt="{ item }">
        {{ formatDate(item.createdAt) }}
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import { NButton } from "naive-ui";
import { ref } from "vue";
import type { Mailing } from "~/api/mailings/types";
import { apiMailingsGetAll } from "~/api/mailings";
import Table from "~/components/Table/Table.vue";
import { formatDate } from "~/utils/date";

const mailings = ref<Mailing[]>([]);

apiMailingsGetAll().then((v) => (mailings.value = v.data));
</script>
