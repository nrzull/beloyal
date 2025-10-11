<template>
  <Page>
    <template #top>
      <Heading>Роли</Heading>
      <Button
        type="primary"
        size="medium"
        @click="$router.push('/company-roles/new')"
      >
        Добавить роль
      </Button>
    </template>

    <Table
      :data="companyRoles"
      :columns="[
        { key: 'name', title: 'Название' },
        { key: 'createdAt', title: 'Дата создания' },
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
import { formatDate, Table, Button, Heading } from "@beloyal/shared";
import Page from "~/components/Page.vue";

const companyRoles = ref<CompanyRole[]>([]);

apiCompanyRolesGetAll().then((v) => (companyRoles.value = v.data));
</script>
