<template>
  <Page>
    <template #top>
      <PageHeading>Новая роль</PageHeading>
    </template>

    <Card class="v-space-24">
      <RoleForm
        :loading="isLoading"
        v-model:name="name"
        :available-permissions="availablePermissions"
        v-model:selected-permissions="activePermissions"
      />

      <NButton :disabled="isLoading" @click="createRole" type="primary">
        Добавить
      </NButton>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import { NButton } from "naive-ui";
import { computed, ref } from "vue";
import Card from "~/components/Card/Card.vue";
import {
  apiCompanyRolesGetPermissions,
  apiCompanyRolesCreate,
} from "~/api/companyRoles";
import { wait } from "~/composables/wait";
import { useRouter } from "vue-router";
import RoleForm from "~/components/RoleForm/RoleForm.vue";

const name = ref<string>();
const availablePermissions = ref<string[]>([]);
const isLoading = computed(() => wait.some([createRole]));
const activePermissions = ref<string[]>([]);
const router = useRouter();

apiCompanyRolesGetPermissions().then(
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

    router.push("/company-roles");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(createRole);
  }
};
</script>
