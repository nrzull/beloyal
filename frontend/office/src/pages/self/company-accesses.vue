<template>
  <Page>
    <template #top>
      <Heading>Компании</Heading>
    </template>

    <Table
      :columns="[
        { key: 'company', title: 'Компания' },
        { key: 'tin', title: 'ИНН' },
        { key: 'role', title: 'Роль' },
        { key: 'actions', width: 'max-content' },
      ]"
      :data="self?.companyAccesses || []"
    >
      <template #tin="{ item }">
        {{ item.company.tin }}
      </template>

      <template #company="{ item }">
        {{ item.company.title }}
      </template>

      <template #role="{ item }">
        {{ item.role.name }}
      </template>

      <template #actions="{ item }">
        <NButton
          disabled
          v-if="item.id === self?.companyAccess?.id"
          size="small"
          type="primary"
        >
          Активен
        </NButton>
        <NButton v-else size="small" @click="setCompanyAccess(item.id)">
          Перейти
        </NButton>
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, wait } from "@beloyal/shared";
import Table from "~/components/Table/Table.vue";
import { self } from "~/composables/self";
import { NButton } from "naive-ui";
import { apiSelfSetCompanyAccess } from "~/api/self";

const setCompanyAccess = async (companyAccessId: string) => {
  try {
    if (companyAccessId && self.value?.companyAccess?.id === companyAccessId) {
      return;
    }

    wait.start(setCompanyAccess);
    await apiSelfSetCompanyAccess({ companyAccessId });
    location.reload();
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(setCompanyAccess);
  }
};
</script>
