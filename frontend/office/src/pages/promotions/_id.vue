<template>
  <Page>
    <template #top>
      <Heading>Редактирование акции</Heading>
    </template>

    <Card v-if="promotion" class="v-space-12">
      <PromotionForm v-model="promotion" :loading="isLoading" />
      <Button :disabled="isLoading" type="primary" @click="updatePromotion">
        Сохранить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, Button, Card, wait, type Promotion } from "@beloyal/shared";
import { computed, ref } from "vue";
import PromotionForm from "~/components/Promotions/PromotionForm.vue";
import { apiPromotionsGet, apiPromotionsUpdate } from "~/api/promotions";
import { useRoute, useRouter } from "vue-router";

const isLoading = computed(() => wait.some([updatePromotion]));

const promotion = ref<Promotion>();
const router = useRouter();
const route = useRoute();

const id = computed(() => route.params.id as string);

const updatePromotion = async () => {
  if (!promotion.value) {
    return;
  }

  try {
    wait.start(updatePromotion);
    await apiPromotionsUpdate(promotion.value);
    router.push("/promotions");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(updatePromotion);
  }
};

apiPromotionsGet({ id: id.value }).then((v) => (promotion.value = v.data));
</script>
