<template>
  <Page>
    <template #top>
      <PageHeading>Роли</PageHeading>
      <NButton
        type="primary"
        size="medium"
        @click="$router.push('/company-roles/new')"
      >
        Добавить роль
      </NButton>
    </template>

    <Table
      :data="companyRoles"
      :columns="[
        { key: 'name', title: 'Название' },
        { key: 'createdAt', title: 'Создано' },
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
import { ref } from "vue";
import { apiCompanyRolesGetAll } from "~/api/companyRoles";
import type { CompanyRole } from "@beloyal/shared";
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Table from "~/components/Table/Table.vue";
import { formatDate } from "~/utils/date";
import { NButton } from "naive-ui";

const companyRoles = ref<CompanyRole[]>([]);

apiCompanyRolesGetAll().then((v) => (companyRoles.value = v.data));
</script>
