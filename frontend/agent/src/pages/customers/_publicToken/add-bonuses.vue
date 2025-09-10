<template>
  <Card>
    <NThing>
      <template #header>
        <BackButton @click="$router.push(`/customers/${publicToken}`)">
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
            placeholder="Введите сумму оплаты"
          >
            <template #prefix>
              <Icon variant="rub" />
            </template>
          </NInputNumber>
        </NSpace>

        <NSpace vertical>
          <div>Комментарий <b>(необязательно)</b></div>
          <NInput type="textarea" placeholder="Введите комментарий" />
        </NSpace>
      </NSpace>

      <template #footer>
        <NButton block size="large" type="primary">Начислить бонусы</NButton>
      </template>
    </NThing>
  </Card>
</template>

<script setup lang="ts">
import type { Customer } from "~/api/customers";
import { NInputNumber, NSpace, NInput, NButton, NThing } from "naive-ui";
import { Card, Icon, Heading } from "@beloyal/shared";
import BackButton from "~/components/BackButton.vue";

defineProps<{ customer: Customer; publicToken: string }>();
</script>
