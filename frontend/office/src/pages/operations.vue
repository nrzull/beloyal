<template>
  <Page>
    <template #top>
      <Heading>Операции</Heading>
    </template>

    <Table
      :data="operations"
      :columns="[
        { key: 'createdAt', title: 'Дата' },
        { key: 'type', title: 'Операция' },
        { key: 'cashIncome', title: 'Прибыль' },
        { key: 'shop', title: 'Торговая точка' },
        { key: 'agent', title: 'Сотрудник' },
      ]"
    >
      <template #type="{ item }">
        {{ translateBonusType(item.bonus.type) }}
      </template>

      <template #cashIncome="{ item }">
        {{ item.bonus.cashIncome }}
      </template>

      <template #shop="{ item }">
        {{ item.shop?.name }}
      </template>

      <template #agent="{ item }">
        {{
          buildFullName(item.agent?.user) || maskPhone(item.agent?.user?.phone)
        }}
      </template>

      <template #createdAt="{ item }">
        {{ formatDate(item.bonus.createdAt) }}
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import { Heading, type CompanyCashback } from "@beloyal/shared";
import Page from "~/components/Page.vue";
import { apiOperationsGetAll } from "~/api/operations";
import type { Operation } from "~/api/operations/types";
import { ref } from "vue";
import Table from "~/components/Table/Table.vue";
import { formatDate } from "~/utils/date";
import { apiCompanyGetCashback } from "~/api/company";
import { maskPhone, buildFullName } from "@beloyal/shared";
import { translateBonusType } from "@beloyal/shared";

const operations = ref<Operation[]>([]);

const cashback = ref<CompanyCashback>();

apiOperationsGetAll().then((v) => (operations.value = v.data));
apiCompanyGetCashback().then((v) => (cashback.value = v.data));
</script>
