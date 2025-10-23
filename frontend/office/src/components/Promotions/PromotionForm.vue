<template>
  <Space vertical>
    <div>Название акции</div>
    <Input
      :disabled="loading"
      placeholder="Укажите название"
      v-model:value="promotion.title"
    />
  </Space>

  <Space vertical>
    <div>Тип акции</div>
    <NSelect
      :options="options"
      placeholder="Укажите тип акции"
      value-field="$type"
      label-field="$type"
      :value="promotion.variant?.$type"
      :render-label="(v: PromotionVariant) => translatePromotionVariantType[v.$type]"
      @update:value="
        promotion.variant = cloneDeep(options.find((v) => v.$type === $event)!)
      "
    />
  </Space>

  <VariantForm v-if="promotion.variant" v-model="promotion.variant" />
</template>

<script setup lang="ts">
import { Space, Input, type PromotionVariant } from "@beloyal/shared";
import { NSelect } from "naive-ui";
import type { PartialPromotion } from "~/components/Promotions/types";
import {
  newDatePeriodVariant,
  newDateTimePeriodVariant,
  newExactCountVariant,
  newOperationsInactivityVariant,
  newTimePeriodVariant,
  newMoneySpentVariant,
  translatePromotionVariantType,
} from "~/components/Promotions/utils";
import { cloneDeep } from "es-toolkit";
import VariantForm from "~/components/Promotions/VariantForm.vue";

defineProps<{ loading: boolean }>();

const options = [
  newExactCountVariant(),
  newDatePeriodVariant(),
  newDateTimePeriodVariant(),
  newTimePeriodVariant(),
  newOperationsInactivityVariant(),
  newMoneySpentVariant(),
];

const promotion = defineModel<PartialPromotion>({ required: true });
</script>
