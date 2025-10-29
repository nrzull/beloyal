<template>
  <Page :loading="isLoading">
    <template #top>
      <NBreadcrumb>
        <NBreadcrumbItem @click="$router.push('/company-accesses')">
          Пользователи
        </NBreadcrumbItem>
        <NBreadcrumbItem>{{
          maskPhone(companyAccess?.user?.phone!)
        }}</NBreadcrumbItem>
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

      <Button :disabled="isLoading" type="primary" @click="updateAccess">
        Сохранить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { apiCompanyRolesGetAll } from "~/api/companyRoles";
import type { CompanyRole } from "@beloyal/shared";
import Page from "~/components/Page.vue";
import { NBreadcrumb, NBreadcrumbItem, NSelect } from "naive-ui";
import type { CompanyAccess } from "@beloyal/shared";
import {
  apiCompanyAccessesGet,
  apiCompanyAccessesUpdate,
} from "~/api/companyAccesses";
import { useRoute, useRouter } from "vue-router";
import { Card, useToast, waitable } from "@beloyal/shared";
import { wait } from "@beloyal/shared";
import { maskPhone, Button } from "@beloyal/shared";

const route = useRoute();
const router = useRouter();
const companyAccess = ref<CompanyAccess>();
const companyRoles = ref<CompanyRole[]>([]);
const companyAccessId = computed(() => route.params.id as string);
const { showOk, showError } = useToast();
const isLoading = computed(() =>
  wait.some([apiCompanyRolesGetAll, apiCompanyAccessesGet, updateAccess])
);

const updateAccess = async () => {
  try {
    wait.start(updateAccess);

    await apiCompanyAccessesUpdate({
      roleId: companyAccess.value?.role.id!,
      companyAccessId: companyAccessId.value,
    });

    showOk();
    router.push("/company-accesses");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(updateAccess);
  }
};

waitable(apiCompanyRolesGetAll)().then((v) => (companyRoles.value = v.data));

waitable(apiCompanyAccessesGet)({
  companyAccessId: companyAccessId.value,
}).then((v) => (companyAccess.value = v.data));
</script>
