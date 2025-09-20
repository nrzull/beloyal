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

      <Space vertical :size="[0, 16]">
        <Space vertical>
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
        </Space>

        <Space vertical>
          <div>Комментарий <b>(необязательно)</b></div>
          <Input
            :disabled="isLoading"
            type="textarea"
            placeholder="Введите комментарий"
          />
        </Space>
      </Space>

      <template #footer>
        <Button
          :disabled="isLoading"
          block
          size="large"
          type="primary"
          @click="addBonus"
        >
          Начислить бонусы
        </Button>
      </template>
    </NThing>
  </Card>
</template>

<script setup lang="ts">
import type { Customer } from "~/api/customers";
import { NInputNumber, NThing } from "naive-ui";
import {
  Space,
  Card,
  Icon,
  Heading,
  wait,
  Button,
  Input,
} from "@beloyal/shared";
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

    const result = await apiCustomersAddBonus({
      publicToken,
      income: income.value,
    });

    emit("refresh");

    router.push({
      path: `/customers/${publicToken}/final`,
      query: result.data,
    });
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(addBonus);
  }
};
</script>
