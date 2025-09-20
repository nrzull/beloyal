<template>
  <Page>
    <template #top>
      <PageHeading>Рассылки</PageHeading>

      <Button
        type="primary"
        size="medium"
        @click="$router.push('/mailings/new')"
      >
        Новая рассылка
      </Button>
    </template>

    <Table
      :data="mailings"
      :columns="[
        { key: 'createdAt', title: 'Создано' },
        { key: 'done', title: 'Получено' },
        { key: 'cancelled', title: 'Отменено' },
        { key: 'total', title: 'Всего' },
      ]"
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
import { ref } from "vue";
import type { Mailing } from "@beloyal/shared";
import { Button } from "@beloyal/shared";
import { apiMailingsGetAll } from "~/api/mailings";
import Table from "~/components/Table/Table.vue";
import { formatDate } from "~/utils/date";

const mailings = ref<Mailing[]>([]);

apiMailingsGetAll().then((v) => (mailings.value = v.data));
</script>
