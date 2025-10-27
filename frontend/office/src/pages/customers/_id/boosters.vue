<template>
  <div>
    <div class="v-space-24">
      <div :class="$style.top">
        <slot name="tabs"></slot>
        <Button type="primary" @click="createBoosterModal = newBooster()">
          Добавить бустер
        </Button>
      </div>

      <Table
        :loading="isLoading"
        :data="boosters"
        :columns="[
          { key: 'createdAt', title: 'Дата создания' },
          { key: 'expiredAt', title: 'Срок действия' },
          { key: 'isActive', title: 'Активный' },
          { key: 'cashbackPercent', title: 'Кэшбэк' },
        ]"
      >
        <template #createdAt="{ item }">
          {{ formatDate(item.createdAt) }}
        </template>

        <template #expiredAt="{ item }">
          {{ formatDate(item.expiredAt) }}
        </template>

        <template #isActive="{ item }">
          {{ item.isActive ? "+" : "-" }}
        </template>

        <template #cashbackPercent="{ item }">
          {{ item.cashbackPercent }}%
        </template>
      </Table>
    </div>

    <NModal
      :class="$style.modal"
      v-if="createBoosterModal"
      show
      @update:show="createBoosterModal = undefined"
      preset="card"
    >
      <template #header>Добавить бустер</template>
      <BoosterForm v-model="createBoosterModal" />

      <template #footer>
        <Button type="primary" @click="createBooster" :disabled="isLoading">
          Добавить
        </Button>
      </template>
    </NModal>
  </div>
</template>

<script setup lang="ts">
import type { Booster, Customer } from "@beloyal/shared";
import { computed, ref } from "vue";
import {
  apiCustomersGetBoosters,
  apiCustomersCreateBooster,
} from "~/api/customers";
import { Table, Button, wait } from "@beloyal/shared";
import { NModal } from "naive-ui";
import BoosterForm from "~/components/Boosters/BoosterForm.vue";
import { newBooster } from "~/components/Boosters/utils";
import { formatDate } from "@beloyal/shared";

const { customer, loading } = defineProps<{
  customer: Customer;
  loading: boolean;
}>();

const isLoading = computed(
  () => loading || wait.some([createBooster, fetchBoosters])
);

const boosters = ref<Booster[]>([]);
const createBoosterModal =
  ref<Pick<Booster, "expiredAt" | "cashbackPercent">>();

const createBooster = async () => {
  try {
    wait.start(createBooster);
    await apiCustomersCreateBooster({
      customerId: customer.id,
      ...createBoosterModal.value!,
    });
    await fetchBoosters();
    createBoosterModal.value = undefined;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(createBooster);
  }
};

const fetchBoosters = async () => {
  try {
    wait.start(fetchBoosters);
    const result = await apiCustomersGetBoosters({ customerId: customer.id });
    boosters.value = result.data;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(fetchBoosters);
  }
};

fetchBoosters();
</script>

<style module>
.top {
  display: flex;
}

.modal {
  max-width: 720px;
}
</style>
