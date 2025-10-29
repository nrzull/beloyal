<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Новая акция</Heading>
    </template>

    <Card class="v-space-12">
      <PromotionForm v-model="promotion" :loading="isLoading" />
      <Button :disabled="isLoading" type="primary" @click="createPromotion">
        Создать
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import {
  Heading,
  Button,
  Card,
  wait,
  type Promotion,
  useToast,
} from "@beloyal/shared";
import { computed, ref } from "vue";
import type { PartialPromotion } from "~/components/Promotions/types";
import { apiPromotionsCreate } from "~/api/promotions";
import { useRouter } from "vue-router";
import PromotionForm from "~/components/Promotions/PromotionForm.vue";

const isLoading = computed(() => wait.some([createPromotion]));
const { showOk, showError } = useToast();
const promotion = ref<PartialPromotion>({ title: "", variant: null });
const router = useRouter();

const createPromotion = async () => {
  try {
    wait.start(createPromotion);
    await apiPromotionsCreate(promotion.value as Promotion);
    showOk();
    router.push("/promotions");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(createPromotion);
  }
};
</script>
