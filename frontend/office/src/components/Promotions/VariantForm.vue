<template>
  <component :is="computedComponent" v-model="(model as any)" />
</template>

<script setup lang="ts">
import ExactCountVariantForm from "~/components/Promotions/ExactCountVariantForm.vue";
import DatePeriodVariantForm from "~/components/Promotions/DatePeriodVariantForm.vue";
import TimePeriodVariantForm from "~/components/Promotions/TimePeriodVariantForm.vue";
import DateTimePeriodVariantForm from "~/components/Promotions/DateTimePeriodVariantForm.vue";
import OperationsInactivityVariantForm from "~/components/Promotions/OperationsInactivityVariantForm.vue";
import { switchGuard, type PromotionVariant } from "@beloyal/shared";
import { computed } from "vue";

const model = defineModel<PromotionVariant>({ required: true });

const computedComponent = computed(() => {
  switch (model.value.$type) {
    case "ExactCountVariant":
      return ExactCountVariantForm;

    case "DatePeriodVariant":
      return DatePeriodVariantForm;

    case "DateTimePeriodVariant":
      return DateTimePeriodVariantForm;

    case "TimePeriodVariant":
      return TimePeriodVariantForm;

    case "OperationsInactivityVariant":
      return OperationsInactivityVariantForm;

    default:
      throw switchGuard(model.value);
  }
});
</script>
