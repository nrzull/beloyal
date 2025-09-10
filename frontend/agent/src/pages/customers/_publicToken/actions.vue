<template>
  <Card>
    <NSpace vertical>
      <NAlert type="info" title="Бонусы">
        <template #icon>
          <Icon variant="coins" />
        </template>

        У клиента <b>{{ customer.data.bonusAmount }}</b> Бонусов
      </NAlert>

      <RouterView :customer="customer" />

      <NButton
        block
        type="primary"
        size="large"
        @click="$router.push(`/customers/${publicToken}/add-bonuses`)"
      >
        Начислить
      </NButton>

      <NButton
        block
        v-if="customer.data.bonusAmount > 0"
        @click="$router.push(`/customers/${publicToken}/subtract-bonuses`)"
      >
        Списать
      </NButton>
    </NSpace>
  </Card>
</template>

<script setup lang="ts">
import { type Customer } from "~/api/customers";
import { NButton, NSpace, NAlert } from "naive-ui";
import { Card, Icon } from "@beloyal/shared";

defineProps<{ customer: Customer; publicToken: string }>();
</script>
