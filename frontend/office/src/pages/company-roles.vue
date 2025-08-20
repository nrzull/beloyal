<template>
  <Page>
    <template #top>
      <PageHeading>Роли</PageHeading>
      <Button size="m" @click="$router.push('/company-roles/new')">
        Добавить роль
      </Button>
    </template>

    <Table
      :data="companyRoles"
      :columns="[
        { key: 'name', title: 'Название' },
        { key: 'createdAt', title: 'Добавлено' },
      ]"
    >
      <template #id="{ item }">
        {{ item.id }}
      </template>

      <template #createdAt="{ item }">
        {{ formatDate(item.createdAt) }}
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { apiCompanyRolesGetAll } from "~/api/companyRoles";
import type { CompanyRole } from "~/api/companyRoles/types";
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Table from "~/components/Table/Table.vue";
import { formatDate } from "~/utils/date";
import Button from "~/components/Button/Button.vue";

const companyRoles = ref<CompanyRole[]>([]);

apiCompanyRolesGetAll().then((v) => (companyRoles.value = v.data));
</script>
