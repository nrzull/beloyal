<template>
  <NThing>
    <template #header>
      <Heading>Операции</Heading>
    </template>

    <Table
      :with-header="false"
      :data="operations"
      :loading="isLoading"
      :columns="[
        { key: 'createdAt', width: 'max-content' },
        { key: 'cashIncome' },
        { key: 'agent' },
        { key: 'actions', width: 'max-content' },
      ]"
    >
      <template #cashIncome="{ item }">
        {{ item.bonus.cashIncome }}
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
          ghost
          :bordered="false"
          :disabled="isLoading"
        >
          <template #icon>
            <Icon
              variant="cancel"
              @click="
                dialog.error({
                  title: 'Отмена заказа',
                  positiveText: 'отменить',
                  negativeText: 'назад',
                  content: 'Вы действительно хотите отменить заказ?',
                  onPositiveClick: () => cancel(item),
                })
              "
            />
          </template>
        </Button>
      </template>
    </Table>
  </NThing>
</template>

<script setup lang="ts">
import {
  formatDate,
  Table,
  Button,
  Heading,
  wait,
  Icon,
  type Operation,
} from "@beloyal/shared";
import { apiOperationsCancel, apiOperationsGetAll } from "~/api/operations";
import { computed, ref } from "vue";
import { maskPhone, buildFullName } from "@beloyal/shared";
import { NThing } from "naive-ui";
import { useDialog } from "naive-ui";

const operations = ref<Operation[]>([]);
const dialog = useDialog();

const isLoading = computed(() => wait.some([fetchOperations]));

const fetchOperations = async () => {
  try {
    wait.start(fetchOperations);
    const result = await apiOperationsGetAll();
    operations.value = result.data;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(fetchOperations);
  }
};

const cancel = async (v: Operation) => {
  try {
    wait.start(cancel);

    await apiOperationsCancel({ bonusId: v.bonus.id });
    await fetchOperations();
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(cancel);
  }
};

fetchOperations();
</script>
