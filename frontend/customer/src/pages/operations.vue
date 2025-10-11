<template>
  <NThing>
    <template #header>
      <Heading>История</Heading>
    </template>

    <Table
      :with-header="false"
      :data="operations"
      :loading="isLoading"
      :columns="[
        { key: 'createdAt', width: 'max-content' },
        { key: 'amount', class: [$style.amount] },
        { key: 'agent' },
      ]"
    >
      <template #amount="{ item }">
        {{ item.bonus.amount }}
        <Icon
          :style="{
            color:
              item.bonus.type === 'AddBonus'
                ? 'var(--color-success)'
                : item.bonus.type === 'SubtractBonus'
                ? 'var(--color-error)'
                : item.bonus.type === 'BurnBonus'
                ? 'var(--color-error)'
                : undefined,
          }"
          :variant="
            item.bonus.type === 'AddBonus'
              ? 'chevronUp'
              : item.bonus.type === 'SubtractBonus'
              ? 'chevronDown'
              : item.bonus.type === 'BurnBonus'
              ? 'fire'
              : 'coins'
          "
        />
      </template>

      <template #agent="{ item }">
        {{
          buildFullName(item.agent?.user) || maskPhone(item.agent?.user?.phone)
        }}
      </template>

      <template #createdAt="{ item }">
        {{ formatDate(item.bonus.createdAt) }}
      </template>
    </Table>
  </NThing>
</template>

<script setup lang="ts">
import {
  formatDate,
  Table,
  Heading,
  wait,
  Icon,
  type Operation,
} from "@beloyal/shared";
import { apiOperationsGetAll } from "~/api/operations";
import { computed, ref } from "vue";
import { maskPhone, buildFullName } from "@beloyal/shared";
import { NThing } from "naive-ui";

const operations = ref<Operation[]>([]);

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

fetchOperations();
</script>

<style module>
.amount {
  justify-content: flex-end;
}
</style>
