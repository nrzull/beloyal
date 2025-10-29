<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Новая роль</Heading>
    </template>

    <Card class="v-space-24">
      <RoleForm
        :loading="isLoading"
        v-model:name="name"
        :available-permissions="availablePermissions"
        v-model:selected-permissions="activePermissions"
      />

      <Button :disabled="isLoading" @click="createRole" type="primary">
        Добавить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { computed, ref } from "vue";
import { Heading, Card, Button, useToast, waitable } from "@beloyal/shared";
import {
  apiCompanyRolesGetPermissions,
  apiCompanyRolesCreate,
} from "~/api/companyRoles";
import { wait } from "@beloyal/shared";
import { useRouter } from "vue-router";
import RoleForm from "~/components/RoleForm/RoleForm.vue";

const name = ref<string>();
const availablePermissions = ref<string[]>([]);
const isLoading = computed(() =>
  wait.some([createRole, apiCompanyRolesGetPermissions])
);
const activePermissions = ref<string[]>([]);
const router = useRouter();
const { showOk, showError } = useToast();

waitable(apiCompanyRolesGetPermissions)().then(
  (v) => (availablePermissions.value = v.data)
);

const createRole = async () => {
  if (!name.value) {
    return;
  }

  try {
    wait.start(createRole);

    await apiCompanyRolesCreate({
      name: name.value,
      permissions: activePermissions.value,
    });
    showOk();
    router.push("/company-roles");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(createRole);
  }
};
</script>
