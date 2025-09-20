<template>
  <Card>
    <NThing>
      <template #header>
        <BackButton @click="$router.push('/')">
          <Heading>
            {{
              bonus.type === "AddBonus"
                ? "Зачисление"
                : bonus.type === "SubtractBonus"
                ? "Списание"
                : bonus.type
            }}
          </Heading>
        </BackButton>
      </template>

      <Space vertical :size="[0, 24]">
        <div v-if="bonus.type === 'AddBonus'">
          Зачислено
          <b>{{ Number(bonus.amount) * customer.data.bonusMultiplier }}</b>
          бонусов. Сумма к оплате:
          <b>{{ bonus.cashIncome }}</b>
        </div>

        <div v-if="bonus.type === 'SubtractBonus'">
          Списано
          <b>{{ Number(bonus.amount) * customer.data.bonusMultiplier }}</b>
          бонусов. Сумма к оплате:
          <b>{{ bonus.cashIncome }}</b>
        </div>

        <Button size="large" type="primary" block @click="$router.push('/')">
          На главную
        </Button>
      </Space>
    </NThing>
  </Card>
</template>

<script setup lang="ts">
import { useRoute } from "vue-router";
import { Card, Heading, Space, Button, type Bonus } from "@beloyal/shared";
import { NThing } from "naive-ui";
import type { Customer } from "~/api/customers";

const { customer } = defineProps<{
  customer: Customer;
}>();

const route = useRoute();

const bonus = route.query as unknown as Bonus;
</script>
