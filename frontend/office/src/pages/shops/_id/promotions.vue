<template>
  <div>
    <div class="v-space-24">
      <div :class="$style.top">
        <slot name="tabs"></slot>
        <Button type="primary" @click="addShopPromotionModal = true">
          Настроить акции
        </Button>
      </div>

      <Table
        :data="shopPromotions"
        :loading="isLoading"
        :columns="[
          { key: 'title', title: 'Название' },
          { key: 'variant', title: 'Тип' },
          { key: 'cashbackPercent', title: 'Кэшбэк' },
          { key: 'createdAt', title: 'Дата создания' },
        ]"
        :route-fn="(v) => `/promotions/${v.promotion.id}`"
      >
        <template #title="{ item }">
          {{ item.promotion.title }}
        </template>

        <template #variant="{ item }">
          {{ translatePromotionVariantType[item.promotion.variant.$type] }}
        </template>

        <template #cashbackPercent="{ item }">
          {{ item.promotion.variant.cashbackPercent }}%
        </template>

        <template #createdAt="{ item }">
          {{ formatDate(item.createdAt) }}
        </template>
      </Table>
    </div>

    <NModal
      :class="$style.modal"
      v-model:show="addShopPromotionModal"
      preset="card"
    >
      <template #header>Настроить акции</template>

      <Table
        clickable
        :card="{ withPadding: false, withShadow: false }"
        :data="availablePromotions"
        :selected-fn="
          (v) => shopPromotions.some((vv) => vv.promotion.id === v.id)
        "
        @row="toggleShopPromotion"
        :columns="[
          { key: 'title', title: 'Название' },
          { key: 'variant', title: 'Тип' },
          { key: 'cashbackPercent', title: 'Кэшбэк' },
        ]"
      >
        <template #title="{ item }">
          {{ item.title }}
        </template>

        <template #variant="{ item }">
          {{ translatePromotionVariantType[item.variant.$type] }}
        </template>

        <template #cashbackPercent="{ item }">
          {{ item.variant.cashbackPercent }}%
        </template>
      </Table>
    </NModal>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import {
  Table,
  Button,
  type ShopPromotion,
  type Shop,
  type Promotion,
  wait,
  useToast,
  waitable,
} from "@beloyal/shared";
import {
  apiShopsAddPromotion,
  apiShopsGetPromotions,
  apiShopsRemovePromotion,
} from "~/api/shops";
import { translatePromotionVariantType } from "~/components/Promotions/utils";
import { formatDate } from "@beloyal/shared";
import { apiPromotionsGetAll } from "~/api/promotions";
import { NModal } from "naive-ui";

const { shop, loading } = defineProps<{ shop: Shop; loading: boolean }>();
const { showOk, showError } = useToast();
const shopPromotions = ref<ShopPromotion[]>([]);
const availablePromotions = ref<Promotion[]>([]);
const addShopPromotionModal = ref(false);

const isLoading = computed(
  () =>
    loading ||
    wait.some([
      apiPromotionsGetAll,
      fetchShopPromotions,
      removeShopPromotion,
      addShopPromotion,
    ])
);

const fetchShopPromotions = async () => {
  try {
    wait.start(fetchShopPromotions);
    const result = await apiShopsGetPromotions({ shopId: shop.id });
    shopPromotions.value = result.data;
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(fetchShopPromotions);
  }
};

const removeShopPromotion = async (v: Promotion) => {
  const shopPromotion = shopPromotions.value.find(
    (vv) => vv.promotion.id == v.id
  );

  if (isLoading.value || !shopPromotion) {
    return;
  }

  try {
    wait.start(removeShopPromotion);
    await apiShopsRemovePromotion({
      shopId: shop.id,
      shopPromotionId: shopPromotion?.id,
    });
    await fetchShopPromotions();
    showOk();
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(removeShopPromotion);
  }
};

const toggleShopPromotion = async (v: Promotion) =>
  shopPromotions.value.some((vv) => vv.promotion.id === v.id)
    ? removeShopPromotion(v)
    : addShopPromotion(v);

const addShopPromotion = async (v: Promotion) => {
  if (
    isLoading.value ||
    shopPromotions.value.some((vv) => vv.promotion.id === v.id)
  ) {
    return;
  }

  try {
    wait.start(addShopPromotion);

    await apiShopsAddPromotion({ shopId: shop.id, promotionId: v.id });
    await fetchShopPromotions();
    showOk();
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(addShopPromotion);
  }
};

waitable(apiPromotionsGetAll)().then(
  (v) => (availablePromotions.value = v.data)
);
fetchShopPromotions();
</script>

<style module>
.modal {
  max-width: 720px;
}

.top {
  display: flex;
}
</style>
