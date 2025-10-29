<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Белый список</Heading>
      <Button
        type="primary"
        size="medium"
        @click="$router.push('/white-companies/new')"
      >
        Добавить компанию
      </Button>
    </template>

    <Table
      :data="whiteCompanies"
      :loading="isLoading"
      :columns="[
        { key: 'tin', title: 'ИНН' },
        { key: 'expiredAt', title: 'Срок действия' },
      ]"
      :route-fn="(v) => `/white-companies/${v.id}`"
    >
      <template #expiredAt="{ item }">
        {{
          compareAsc(item.expiredAt, new Date()) === -1
            ? "Исчерпан"
            : formatDistanceStrictDate(new Date(), item.expiredAt)
        }}
        ({{ formatDate(item.expiredAt) }})
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { apiWhiteCompaniesGetAll } from "~/api/whiteCompanies";
import { computed, ref } from "vue";
import type { WhiteCompany } from "@beloyal/shared";
import { Table, Button, Heading, wait, waitable } from "@beloyal/shared";
import { formatDistanceStrictDate, formatDate } from "@beloyal/shared";
import { compareAsc } from "date-fns";

const whiteCompanies = ref<WhiteCompany[]>([]);

const isLoading = computed(() => wait.some([apiWhiteCompaniesGetAll]));

waitable(apiWhiteCompaniesGetAll)().then(
  (v) => (whiteCompanies.value = v.data)
);
</script>
