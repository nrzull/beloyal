<template>
  <Page>
    <template #top>
      <Heading>Компании</Heading>

      <Button
        v-if="self?.adminAccess && self.companyAccess"
        type="primary"
        size="medium"
        @click="setCompanyAccess(null)"
      >
        Войти как администратор
      </Button>
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
        <Button
          disabled
          v-if="item.id === self?.companyAccess?.id"
          size="small"
          type="primary"
        >
          Активен
        </Button>
        <Button v-else size="small" @click="setCompanyAccess(item.id)">
          Войти
        </Button>
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, wait, Button } from "@beloyal/shared";
import Table from "~/components/Table/Table.vue";
import { self } from "~/composables/self";
import { apiSelfSetCompanyAccess } from "~/api/self";

const setCompanyAccess = async (companyAccessId: string | null) => {
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
