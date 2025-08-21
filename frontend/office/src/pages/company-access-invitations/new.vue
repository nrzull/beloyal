<template>
  <Page>
    <template #top>
      <PageHeading>Новое приглашение</PageHeading>
    </template>

    <Card :class="$style.form">
      <Input placeholder="E-mail" v-model="email" />

      <SelectInput
        v-model="selectedRoleId"
        :input="{ placeholder: 'Роль', withClear: true }"
        :menu-list="{
          items: companyRoles,
          idPath: 'id',
          valuePath: 'id',
          labelPath: 'name',
        }"
      />

      <NButton>Пригласить</NButton>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Card from "~/components/Card/Card.vue";
import Input from "~/components/Input/Input.vue";
import { NButton } from "naive-ui";
import { computed, ref } from "vue";
import { wait } from "~/composables/wait";
import { apiCompanyRolesGetAll } from "~/api/companyRoles";
import type { CompanyRole } from "~/api/companyRoles/types";
import SelectInput from "~/components/SelectInput/SelectInput.vue";

const isLoading = computed(() => wait.some([createCompanyAccessInvitation]));

const email = ref<string>();
const selectedRoleId = ref<string>();

const companyRoles = ref<CompanyRole[]>([]);

const createCompanyAccessInvitation = async () => {
  try {
    wait.start(createCompanyAccessInvitation);
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
