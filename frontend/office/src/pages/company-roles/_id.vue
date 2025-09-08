<template>
  <Page>
    <template #top>
      <PageHeading>Редактирование роли</PageHeading>
    </template>

    <Card class="v-space-24">
      <RoleForm
        :loading="isLoading"
        v-model:name="name"
        :available-permissions="availablePermissions"
        v-model:selected-permissions="activePermissions"
      />

      <NButton type="primary" :disabled="isLoading" @click="updateRole">
        Сохранить
      </NButton>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import { NButton } from "naive-ui";
import { computed, ref } from "vue";
import { Card } from "@beloyal/shared";
import {
  apiCompanyRolesGetPermissions,
  apiCompanyRolesGet,
  apiCompanyRolesUpdate,
} from "~/api/companyRoles";
import { wait } from "@beloyal/shared";
import { useRoute, useRouter } from "vue-router";
import RoleForm from "~/components/RoleForm/RoleForm.vue";

const name = ref<string>();
const availablePermissions = ref<string[]>([]);
const isLoading = computed(() => wait.some([updateRole]));
const activePermissions = ref<string[]>([]);
const router = useRouter();
const route = useRoute();

const roleId = computed(() => route.params.id as string);

apiCompanyRolesGetPermissions().then(
  (v) => (availablePermissions.value = v.data)
);

apiCompanyRolesGet({ roleId: roleId.value }).then((v) => {
  name.value = v.data.name;
  activePermissions.value = v.data.permissions;
});

const updateRole = async () => {
  if (!name.value) {
    return;
  }

  try {
    wait.start(updateRole);

    await apiCompanyRolesUpdate({
      id: roleId.value,
      name: name.value,
      permissions: activePermissions.value,
    });

    router.push("/company-roles");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(updateRole);
  }
};
</script>
