<template>
  <Page>
    <template #top>
      <PageHeading>Новый пользователь</PageHeading>
    </template>

    <Card :class="$style.form">
      <Input
        :disabled="isLoading"
        placeholder="Номер телефона"
        v-model:value="phone"
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

      <Button
        :disabled="isLoading"
        type="primary"
        @click="createCompanyAccessInvitation"
      >
        Добавить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import { Input, Card, Button } from "@beloyal/shared";

import { NSelect } from "naive-ui";
import { computed, ref } from "vue";
import { wait } from "@beloyal/shared";
import { apiCompanyRolesGetAll } from "~/api/companyRoles";
import type { CompanyRole } from "@beloyal/shared";
import { useRouter } from "vue-router";
import { apiCompanyAccessesCreate } from "~/api/companyAccesses";

const isLoading = computed(() => wait.some([createCompanyAccessInvitation]));
const router = useRouter();
const phone = ref<string>();
const selectedRoleId = ref<string>();

const companyRoles = ref<CompanyRole[]>([]);

const createCompanyAccessInvitation = async () => {
  if (!phone.value || !selectedRoleId.value) {
    return;
  }

  try {
    wait.start(createCompanyAccessInvitation);

    await apiCompanyAccessesCreate({
      phone: phone.value,
      roleId: selectedRoleId.value,
    });

    router.push("/company-accesses");
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
