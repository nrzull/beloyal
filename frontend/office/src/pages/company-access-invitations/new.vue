<template>
  <Page>
    <template #top>
      <PageHeading>Новое приглашение</PageHeading>
    </template>

    <Card :class="$style.form">
      <NInput
        :disabled="isLoading"
        placeholder="E-mail"
        v-model:value="email"
      />

      <NSelect
        :disabled="isLoading"
        v-model:value="selectedRoleId"
        placeholder="Роль"
        clearable
        value-field="id"
        label-field="name"
        :options="companyRoles"
      />

      <NButton
        :disabled="isLoading"
        type="primary"
        @click="createCompanyAccessInvitation"
        >Пригласить</NButton
      >
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import { Card } from "@beloyal/shared";

import { NButton, NInput, NSelect } from "naive-ui";
import { computed, ref } from "vue";
import { wait } from "@beloyal/shared";
import { apiCompanyRolesGetAll } from "~/api/companyRoles";
import type { CompanyRole } from "~/api/companyRoles/types";
import { apiCompanyAccessInvitationsCreate } from "~/api/companyAccessInvitations";
import { useRouter } from "vue-router";

const isLoading = computed(() => wait.some([createCompanyAccessInvitation]));
const router = useRouter();
const email = ref<string>();
const selectedRoleId = ref<string>();

const companyRoles = ref<CompanyRole[]>([]);

const createCompanyAccessInvitation = async () => {
  if (!email.value || !selectedRoleId.value) {
    return;
  }

  try {
    wait.start(createCompanyAccessInvitation);

    await apiCompanyAccessInvitationsCreate({
      email: email.value,
      roleId: selectedRoleId.value,
    });

    router.push("/company-access-invitations");
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
