<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Операции</Heading>
    </template>

    <Table
      :data="operations"
      :loading="isLoading"
      :columns="[
        { key: 'status', title: 'Статус', width: 'max-content' },
        { key: 'createdAt', title: 'Дата создания' },
        { key: 'type', title: 'Операция' },
        { key: 'cashIncome', title: 'Прибыль' },
        { key: 'shop', title: 'Торговая точка' },
        { key: 'agent', title: 'Сотрудник' },
        { key: 'actions', width: 'max-content' },
      ]"
    >
      <template #type="{ item }">
        {{ translateBonusType(item.bonus.type) }}
      </template>

      <template #status="{ item }">
        <NTag
          round
          size="small"
          :type="
            item.bonus.status === 'Done'
              ? 'success'
              : item.bonus.status === 'Cancelled'
              ? 'error'
              : 'info'
          "
        >
          {{ translateBonusStatus(item.bonus.status) }}
        </NTag>
      </template>

      <template #cashIncome="{ item }">
        {{ item.bonus.cashIncome }}
      </template>

      <template #shop="{ item }">
        {{ item.shop?.name }}
      </template>

      <template #agent="{ item }">
        {{
          buildFullName(item.agent?.user) || maskPhone(item.agent?.user?.phone)
        }}
      </template>

      <template #createdAt="{ item }">
        {{ formatDate(item.bonus.createdAt) }}
      </template>

      <template #actions="{ item }">
        <Button
          v-if="item.bonus.status !== 'Cancelled'"
          size="tiny"
          type="error"
          :disabled="isLoading"
          @click="cancel(item)"
        >
          Отменить
        </Button>
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import {
  Button,
  Heading,
  useToast,
  wait,
  type Operation,
} from "@beloyal/shared";
import Page from "~/components/Page.vue";
import { apiOperationsCancel, apiOperationsGetAll } from "~/api/operations";
import { computed, ref } from "vue";
import { formatDate } from "@beloyal/shared";
import { Table, maskPhone, buildFullName } from "@beloyal/shared";
import { translateBonusType, translateBonusStatus } from "@beloyal/shared";
import { NTag } from "naive-ui";

const operations = ref<Operation[]>([]);
const { showOk, showError } = useToast();
const isLoading = computed(() => wait.some([fetchOperations, cancel]));

const fetchOperations = async () => {
  try {
    wait.start(fetchOperations);
    const result = await apiOperationsGetAll();
    operations.value = result.data;
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(fetchOperations);
  }
};

const cancel = async (v: Operation) => {
  try {
    wait.start(cancel);
    await apiOperationsCancel({ bonusId: v.bonus.id });
    await fetchOperations();
    showOk();
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(cancel);
  }
};

fetchOperations();
</script>
