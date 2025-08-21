<template>
  <Page>
    <template #top>
      <PageHeading>Новая роль</PageHeading>
    </template>

    <Card :class="$style.form">
      <NInput
        :disabled="isLoading"
        placeholder="Название"
        v-model:value="name"
      />

      <div :class="$style.permissions">
        <NButton
          v-for="p in availablePermissions"
          :key="p"
          size="medium"
          :disabled="isLoading"
          :ghost="!isActive(p)"
          type="primary"
          @click="togglePermission(p)"
        >
          {{ p }}
        </NButton>
      </div>

      <NButton :disabled="isLoading" @click="createRole" type="primary">
        Добавить
      </NButton>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import { NButton, NInput } from "naive-ui";
import { computed, ref } from "vue";
import Card from "~/components/Card/Card.vue";
import {
  apiCompanyRolesGetPermissions,
  apiCompanyRolesCreate,
} from "~/api/companyRoles";
import { wait } from "~/composables/wait";
import { useRouter } from "vue-router";

const name = ref<string>();
const availablePermissions = ref<string[]>([]);
const isLoading = computed(() => wait.some([createRole]));
const activePermissions = ref<string[]>([]);
const router = useRouter();

const isActive = (v: string) => activePermissions.value.includes(v);

const togglePermission = (v: string) =>
  (activePermissions.value = activePermissions.value.includes(v)
    ? activePermissions.value.filter((vv) => vv !== v)
    : activePermissions.value.concat([v]));

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
