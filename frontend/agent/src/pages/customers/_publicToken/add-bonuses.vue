<template>
  <Card>
    <NThing>
      <template #header>
        <BackButton
          @click="isLoading ? null : $router.push(`/customers/${publicToken}`)"
        >
          <Heading> Начислить бонусы </Heading>
        </BackButton>
      </template>

      <NSpace vertical :size="[0, 16]">
        <NSpace vertical>
          <div>Сумма оплаты</div>
          <NInputNumber
            :input-props="{ type: 'number' }"
            :step="0"
            :min="0"
            :precision="2"
            :disabled="isLoading"
            placeholder="Введите сумму оплаты"
            v-model:value="income"
          >
            <template #prefix>
              <Icon variant="rub" />
            </template>
          </NInputNumber>
        </NSpace>

        <NSpace vertical>
          <div>Комментарий <b>(необязательно)</b></div>
          <NInput
            :disabled="isLoading"
            type="textarea"
            placeholder="Введите комментарий"
          />
        </NSpace>
      </NSpace>

      <template #footer>
        <NButton
          :disabled="isLoading"
          block
          size="large"
          type="primary"
          @click="addBonus"
        >
          Начислить бонусы
        </NButton>
      </template>
    </NThing>
  </Card>
</template>

<script setup lang="ts">
import type { Customer } from "~/api/customers";
import { NInputNumber, NSpace, NInput, NButton, NThing } from "naive-ui";
import { Card, Icon, Heading, wait } from "@beloyal/shared";
import BackButton from "~/components/BackButton.vue";
import { apiCustomersAddBonus } from "~/api/customers";
import { computed, ref } from "vue";
import { useRouter } from "vue-router";

const { publicToken } = defineProps<{
  customer: Customer;
  publicToken: string;
}>();

const emit = defineEmits<{ refresh: [] }>();

const income = ref<number>();
const isLoading = computed(() => wait.some([addBonus]));
const router = useRouter();

const addBonus = async () => {
  if (!income.value || income.value <= 0) {
    return;
  }

  try {
    wait.start(addBonus);
    await apiCustomersAddBonus({ publicToken, income: income.value });
    emit("refresh");
    router.push(`/customers/${publicToken}`);
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(addBonus);
  }
};
</script>
