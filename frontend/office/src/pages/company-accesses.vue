<template>
  <Page>
    <template #top>
      <Heading>Пользователи</Heading>

      <Button
        type="primary"
        size="medium"
        @click="$router.push('/company-accesses/new')"
      >
        Добавить пользователя
      </Button>
    </template>

    <Table
      :data="companyAccesses"
      :loading="isLoading"
      :columns="[
        { key: 'phone', title: 'Номер телефона' },
        { key: 'name', title: 'Имя' },
        { key: 'role', title: 'Роль' },
        { key: 'actions' },
      ]"
      :route-fn="(v) => `/company-accesses/${v.id}`"
    >
      <template #phone="{ item }">
        {{ maskPhone(item.user.phone) }}
      </template>

      <template #name="{ item }">
        {{ buildFullName(item.user) }}
      </template>

      <template #role="{ item }">
        {{ item.role?.name }}
      </template>

      <template #actions="{ item }">
        <Button
          v-if="!item.blockedAt && item.id != self?.companyAccess?.id"
          size="tiny"
          type="error"
          :disabled="isLoading"
          @click.prevent="block(item)"
        >
          Заблокировать
        </Button>
        <Button
          v-else-if="item.blockedAt"
          size="tiny"
          type="error"
          :disabled="isLoading"
          ghost
          @click.prevent="unblock(item)"
        >
          Разблокировать
        </Button>
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import {
  apiCompanyAccessesGetAll,
  apiCompanyAccessesBlock,
  apiCompanyAccessesUnblock,
} from "~/api/companyAccesses";
import type { CompanyAccess } from "@beloyal/shared";
import Page from "~/components/Page.vue";
import {
  maskPhone,
  Button,
  Heading,
  buildFullName,
  wait,
  Table,
} from "@beloyal/shared";
import { self } from "~/composables/self";

const companyAccesses = ref<CompanyAccess[]>([]);

const isLoading = computed(() => wait.some([fetchAccesses, block, unblock]));

const fetchAccesses = async () => {
  try {
    wait.start(fetchAccesses);

    const result = await apiCompanyAccessesGetAll();
    companyAccesses.value = result.data;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(fetchAccesses);
  }
};

const block = async (v: CompanyAccess) => {
  try {
    wait.start(block);
    await apiCompanyAccessesBlock({ companyAccessId: v.id });
    await fetchAccesses();
  } catch (e) {
    console.error();
  } finally {
    wait.end(block);
  }
};

const unblock = async (v: CompanyAccess) => {
  try {
    wait.start(unblock);
    await apiCompanyAccessesUnblock({ companyAccessId: v.id });
    await fetchAccesses();
  } catch (e) {
    console.error();
  } finally {
    wait.end(unblock);
  }
};

fetchAccesses();
</script>
