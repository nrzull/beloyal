<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Рассылки</Heading>

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
        { key: 'createdAt', title: 'Дата создания' },
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
import { computed, ref } from "vue";
import type { Mailing } from "@beloyal/shared";
import {
  formatDate,
  Table,
  Button,
  Heading,
  wait,
  waitable,
} from "@beloyal/shared";
import { apiMailingsGetAll } from "~/api/mailings";

const mailings = ref<Mailing[]>([]);

const isLoading = computed(() => wait.some([apiMailingsGetAll]));

waitable(apiMailingsGetAll)().then((v) => (mailings.value = v.data));
</script>
