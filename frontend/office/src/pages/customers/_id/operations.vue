<template>
  <div class="v-space-24">
    <div :class="$style.top">
      <slot name="tabs"></slot>
    </div>

    <Table
      :loading="loading"
      :data="operations"
      :columns="[
        { key: 'createdAt', title: 'Дата создания' },
        { key: 'shop', title: 'Торговая точка' },
        { key: 'cashbackPercent', title: 'Кэшбэк' },
        { key: 'promotion', title: 'Акция' },
        { key: 'amount', class: [$style.amount], title: 'Бонусы' },
      ]"
    >
      <template #shop="{ item }">
        {{ item.shop?.name || "-" }}
      </template>

      <template #cashbackPercent="{ item }">
        {{ item.bonus.cashbackPercent }}%
      </template>

      <template #promotion="{ item }">
        {{
          item.bonus.promotionType
            ? translatePromotionVariantType[item.bonus.promotionType]
            : "-"
        }}
      </template>

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

      <template #createdAt="{ item }">
        {{ formatDate(item.bonus.createdAt) }}
      </template>
    </Table>
  </div>
</template>

<script setup lang="ts">
import type { Customer, Operation } from "@beloyal/shared";
import { ref } from "vue";
import { apiCustomersGetOperations } from "~/api/customers";
import { Table, Icon } from "@beloyal/shared";
import { formatDate } from "@beloyal/shared";
import { translatePromotionVariantType } from "~/components/Promotions/utils";

const { customer, loading } = defineProps<{
  customer: Customer;
  loading: boolean;
}>();

const operations = ref<Operation[]>([]);

apiCustomersGetOperations({ customerId: customer.id }).then(
  (v) => (operations.value = v.data)
);
</script>

<style module>
.top {
  display: flex;
}

.amount {
  justify-content: flex-end;
}
</style>
