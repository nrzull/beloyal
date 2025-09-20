<template>
  <Page>
    <template #top>
      <Heading>Пользователи</Heading>

      <Button
        type="primary"
        size="medium"
        @click="$router.push('/company-accesses/new')"
      >
        Добавить пользователя
      </Button>
    </template>

    <Table
      :data="companyAccesses"
      :columns="[
        { key: 'phone', title: 'Номер телефона' },
        { key: 'role', title: 'Роль' },
      ]"
      :route-fn="(v) => `/company-accesses/${v.id}`"
    >
      <template #phone="{ item }">
        {{ maskPhone(item.user.phone) }}
      </template>

      <template #role="{ item }">
        {{ item.role?.name }}
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { apiCompanyAccessesGetAll } from "~/api/companyAccesses";
import type { CompanyAccess } from "@beloyal/shared";
import Page from "~/components/Page.vue";
import Table from "~/components/Table/Table.vue";
import { maskPhone, Button, Heading } from "@beloyal/shared";

const companyAccesses = ref<CompanyAccess[]>([]);

apiCompanyAccessesGetAll().then((v) => (companyAccesses.value = v.data));
</script>
