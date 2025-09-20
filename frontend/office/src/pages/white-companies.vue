<template>
  <Page>
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
import { apiWhiteCompaniesGetAll } from "~/api/whiteCompanies";
import { ref } from "vue";
import type { WhiteCompany } from "@beloyal/shared";
import { Button, Heading } from "@beloyal/shared";
import Table from "~/components/Table/Table.vue";
import { formatDistanceStrictDate, formatDate } from "~/utils/date";

const whiteCompanies = ref<WhiteCompany[]>([]);

apiWhiteCompaniesGetAll().then((v) => (whiteCompanies.value = v.data));
</script>
