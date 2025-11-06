<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Новый пользователь</Heading>
    </template>

    <Card :class="$style.form">
      <Input
        ref="phoneRef"
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
import {
  Input,
  Card,
  Button,
  Heading,
  unmaskPhone,
  useToast,
  waitable,
} from "@beloyal/shared";

import { NSelect } from "naive-ui";
import { computed, ref, useTemplateRef } from "vue";
import { wait, usePhoneMask } from "@beloyal/shared";
import { apiCompanyRolesGetAll } from "~/api/companyRoles";
import type { CompanyRole } from "@beloyal/shared";
import { useRouter } from "vue-router";
import { apiCompanyAccessesCreate } from "~/api/companyAccesses";

const isLoading = computed(() =>
  wait.some([apiCompanyRolesGetAll, createCompanyAccessInvitation])
);
const router = useRouter();
const phone = ref<string>();
const selectedRoleId = ref<string>();
const { showOk, showError } = useToast();

const phoneRef = useTemplateRef("phoneRef");
usePhoneMask(computed(() => phoneRef.value?.inputElRef!));

const companyRoles = ref<CompanyRole[]>([]);

const createCompanyAccessInvitation = async () => {
  const unmaskedPhone = unmaskPhone(phone.value);

  if (!unmaskedPhone || !selectedRoleId.value) {
    return;
  }

  try {
    wait.start(createCompanyAccessInvitation);

    await apiCompanyAccessesCreate({
      phone: unmaskedPhone,
      roleId: selectedRoleId.value,
    });

    showOk();
    router.push("/company-accesses");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(createCompanyAccessInvitation);
  }
};

waitable(apiCompanyRolesGetAll)().then((v) => (companyRoles.value = v.data));
</script>

<style module>
.form {
  display: grid;
  gap: 12px;
}
</style>
