<template>
  <Page>
    <template #top>
      <NBreadcrumb>
        <NBreadcrumbItem @click="$router.push('/company-accesses')">
          Пользователи
        </NBreadcrumbItem>
        <NBreadcrumbItem>{{ companyAccess?.user?.email }}</NBreadcrumbItem>
      </NBreadcrumb>
    </template>

    <Card v-if="companyAccess" class="v-space-12">
      <NSelect
        :disabled="isLoading"
        v-model:value="companyAccess.role.id"
        placeholder="Роль"
        :clearable="false"
        value-field="id"
        label-field="name"
        :options="companyRoles"
      />

      <NButton :disabled="isLoading" type="primary" @click="updateAccess">
        Сохранить
      </NButton>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { apiCompanyRolesGetAll } from "~/api/companyRoles";
import type { CompanyRole } from "@beloyal/shared";
import Page from "~/components/Page.vue";
import { NBreadcrumb, NBreadcrumbItem, NSelect, NButton } from "naive-ui";
import type { CompanyAccess } from "@beloyal/shared";
import {
  apiCompanyAccessesGet,
  apiCompanyAccessesUpdate,
} from "~/api/companyAccesses";
import { useRoute, useRouter } from "vue-router";
import { Card } from "@beloyal/shared";
import { wait } from "@beloyal/shared";

const route = useRoute();
const router = useRouter();
const companyAccess = ref<CompanyAccess>();
const companyRoles = ref<CompanyRole[]>([]);
const companyAccessId = computed(() => route.params.id as string);

const isLoading = computed(() => wait.some([updateAccess]));

const updateAccess = async () => {
  try {
    wait.start(updateAccess);

    await apiCompanyAccessesUpdate({
      roleId: companyAccess.value?.role.id!,
      companyAccessId: companyAccessId.value,
    });

    router.push("/company-accesses");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(updateAccess);
  }
};

apiCompanyRolesGetAll().then((v) => (companyRoles.value = v.data));

apiCompanyAccessesGet({ companyAccessId: companyAccessId.value }).then(
  (v) => (companyAccess.value = v.data)
);
</script>
