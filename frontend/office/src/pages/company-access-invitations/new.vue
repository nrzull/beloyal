<template>
  <Page>
    <template #top>
      <PageHeading>Новое приглашение</PageHeading>
    </template>

    <Card :class="$style.form">
      <NInput placeholder="E-mail" v-model:value="email" />

      <NSelect
        v-model="selectedRoleId"
        placeholder="Роль"
        clearable
        value-field="id"
        label-field="name"
        :options="companyRoles"
      />

      <NButton type="primary">Пригласить</NButton>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Card from "~/components/Card/Card.vue";

import { NButton, NInput, NSelect } from "naive-ui";
import { computed, ref } from "vue";
import { wait } from "~/composables/wait";
import { apiCompanyRolesGetAll } from "~/api/companyRoles";
import type { CompanyRole } from "~/api/companyRoles/types";
import SelectInput from "~/components/SelectInput/SelectInput.vue";

const isLoading = computed(() => wait.some([createCompanyAccessInvitation]));

const email = ref<string>();
const selectedRoleId = ref<string>();

const companyRoles = ref<CompanyRole[]>([]);

const createCompanyAccessInvitation = async () => {
  try {
    wait.start(createCompanyAccessInvitation);
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(createCompanyAccessInvitation);
  }
};

apiCompanyRolesGetAll().then((v) => (companyRoles.value = v.data));
</script>

<style module>
.form {
  display: grid;
  gap: 12px;
}
</style>
