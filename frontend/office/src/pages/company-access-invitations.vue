<template>
  <Page>
    <template #top>
      <PageHeading>Приглашения</PageHeading>
      <NButton
        size="medium"
        type="primary"
        @click="$router.push('/company-access-invitations/new')"
      >
        Создать приглашение
      </NButton>
    </template>

    <Table
      :data="invitations"
      :columns="[
        { key: 'phone', title: 'Номер телефона' },
        { key: 'role', title: 'Роль' },
        { key: 'status', title: 'Статус' },
        { key: 'updatedAt', title: 'Обновлено' },
      ]"
    >
      <template #role="{ item }">
        {{ item.role?.name }}
      </template>

      <template #status="{ item }">
        <NBadge type="info" :value="item.status" />
      </template>

      <template #updatedAt="{ item }">
        {{ formatDate(item.updatedAt) }}
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import { NButton, NBadge } from "naive-ui";
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Table from "~/components/Table/Table.vue";
import { formatDate } from "~/utils/date";
import { apiCompanyAccessInvitationsGetAll } from "~/api/companyAccessInvitations";
import type { CompanyAccessInvitation } from "@beloyal/shared";
import { ref } from "vue";

const invitations = ref<CompanyAccessInvitation[]>([]);

apiCompanyAccessInvitationsGetAll().then((v) => (invitations.value = v.data));
</script>
