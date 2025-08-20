<template>
  <Page>
    <template #top>
      <PageHeading>Редактирование роли</PageHeading>
    </template>

    <Card :class="$style.form">
      <Input :disabled="isLoading" placeholder="Название" v-model="name" />

      <div :class="$style.permissions">
        <Button
          v-for="p in availablePermissions"
          :key="p"
          size="m"
          :disabled="isLoading"
          :type="isActive(p) ? 'primary' : 'outline-primary'"
          @click="togglePermission(p)"
        >
          {{ p }}
        </Button>
      </div>

      <Button :disabled="isLoading" @click="updateRole">Сохранить</Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Input from "~/components/Input/Input.vue";
import Button from "~/components/Button/Button.vue";
import { computed, ref } from "vue";
import Card from "~/components/Card/Card.vue";
import {
  apiCompanyRolesGetPermissions,
  apiCompanyRolesGet,
  apiCompanyRolesUpdate,
} from "~/api/companyRoles";
import { wait } from "~/composables/wait";
import { useRoute, useRouter } from "vue-router";

const name = ref<string>();
const availablePermissions = ref<string[]>([]);
const isLoading = computed(() => wait.some([updateRole]));
const activePermissions = ref<string[]>([]);
const router = useRouter();
const route = useRoute();

const roleId = computed(() => route.params.id as string);

const isActive = (v: string) => activePermissions.value.includes(v);

const togglePermission = (v: string) =>
  (activePermissions.value = activePermissions.value.includes(v)
    ? activePermissions.value.filter((vv) => vv !== v)
    : activePermissions.value.concat([v]));

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

<style module>
.form {
  display: grid;
  gap: 12px;
}

.permissions {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
}
</style>
