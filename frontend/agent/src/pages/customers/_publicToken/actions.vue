<template>
  <Card>
    <Space vertical>
      <NAlert type="info" title="Бонусы">
        <template #icon>
          <Icon variant="coins" />
        </template>

        У клиента
        <b>{{ customer.data.bonusAmount * customer.data.bonusMultiplier }}</b>
        Бонусов
      </NAlert>

      <RouterView :customer="customer" />

      <Button
        block
        type="primary"
        size="large"
        @click="$router.push(`/customers/${publicToken}/add-bonuses`)"
      >
        Начислить
      </Button>

      <Button
        block
        v-if="customer.data.bonusAmount > 0"
        @click="$router.push(`/customers/${publicToken}/subtract-bonuses`)"
      >
        Списать
      </Button>
    </Space>
  </Card>
</template>

<script setup lang="ts">
import { type Customer } from "~/api/customers";
import { NAlert } from "naive-ui";
import { Space, Card, Icon, Button } from "@beloyal/shared";

defineProps<{ customer: Customer; publicToken: string }>();
</script>
