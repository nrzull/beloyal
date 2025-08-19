<template>
  <Page>
    <template #top>
      <PageHeading>Белый список</PageHeading>
      <Button size="m" @click="$router.push('/white-companies/new')">
        Добавить компанию
      </Button>
    </template>

    <Table
      :data="whiteCompanies"
      :columns="[
        { key: 'tin', title: 'ИНН' },
        { key: 'expiredAt', title: 'Срок действия' },
      ]"
    >
      <template #expiredAt="{ item }">
        {{ formatDistanceStrictDate(new Date(), item.expiredAt) }} ({{
          formatDate(item.expiredAt)
        }})
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Button from "~/components/Button/Button.vue";
import { apiWhiteCompaniesGetAll } from "~/api/whiteCompanies";
import { ref } from "vue";
import type { WhiteCompany } from "~/api/whiteCompanies/types";
import Table from "~/components/Table/Table.vue";
import { formatDistanceStrictDate, formatDate } from "~/utils/date";

const whiteCompanies = ref<WhiteCompany[]>([]);

apiWhiteCompaniesGetAll().then((v) => (whiteCompanies.value = v.data));
</script>
