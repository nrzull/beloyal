<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Редактирование роли</Heading>
    </template>

    <Card class="v-space-24">
      <RoleForm
        :loading="isLoading"
        v-model:name="name"
        :available-permissions="availablePermissions"
        v-model:selected-permissions="activePermissions"
      />

      <Button type="primary" :disabled="isLoading" @click="updateRole">
        Сохранить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { computed, ref } from "vue";
import { Card, Button, Heading, useToast, waitable } from "@beloyal/shared";
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
const isLoading = computed(() =>
  wait.some([apiCompanyRolesGetPermissions, updateRole, apiCompanyRolesGet])
);
const activePermissions = ref<string[]>([]);
const router = useRouter();
const route = useRoute();
const { showOk, showError } = useToast();
const roleId = computed(() => route.params.id as string);

waitable(apiCompanyRolesGetPermissions)().then(
  (v) => (availablePermissions.value = v.data)
);

waitable(apiCompanyRolesGet)({ roleId: roleId.value }).then((v) => {
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
    showOk();
    router.push("/company-roles");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(updateRole);
  }
};
</script>
