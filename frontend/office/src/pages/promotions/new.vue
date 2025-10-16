<template>
  <Page>
    <template #top>
      <Heading>Новая акция</Heading>
    </template>

    <Card class="v-space-12">
      <Space vertical>
        <div>Название акции</div>
        <Input
          :disabled="isLoading"
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
          :render-label="(v: PromotionVariant) => translatePromotionVariantType[v.$type]"
          @update:value="
            promotion.variant = cloneDeep(
              options.find((v) => v.$type === $event)!
            )
          "
        />
      </Space>

      <VariantForm v-if="promotion.variant" v-model="promotion.variant" />

      <Button :disabled="isLoading" type="primary" @click="createPromotion">
        Создать
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import {
  Space,
  Heading,
  Button,
  Card,
  wait,
  Input,
  type PromotionVariant,
  type Promotion,
} from "@beloyal/shared";
import { computed, ref } from "vue";
import { NSelect } from "naive-ui";
import type { PartialPromotion } from "~/components/Promotions/types";
import {
  newExactCountVariant,
  newDatePeriodVariant,
  newDateTimePeriodVariant,
  newTimePeriodVariant,
} from "~/components/Promotions/utils";
import { cloneDeep } from "es-toolkit";
import VariantForm from "~/components/Promotions/VariantForm.vue";
import { translatePromotionVariantType } from "~/components/Promotions/utils";
import { apiPromotionsCreate } from "~/api/promotions";
import { useRouter } from "vue-router";

const isLoading = computed(() => wait.some([createPromotion]));

const promotion = ref<PartialPromotion>({ title: "", variant: null });
const router = useRouter();

const options = [
  newExactCountVariant(),
  newDatePeriodVariant(),
  newDateTimePeriodVariant(),
  newTimePeriodVariant(),
];

const createPromotion = async () => {
  try {
    wait.start(createPromotion);
    await apiPromotionsCreate(promotion.value as Promotion);
    router.push("/promotions");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(createPromotion);
  }
};
</script>
