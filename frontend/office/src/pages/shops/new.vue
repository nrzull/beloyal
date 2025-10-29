<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Новая торговая точка</Heading>
    </template>

    <Card class="v-space-24">
      <ShopForm v-model="model" :loading="isLoading" />

      <Button :disabled="isLoading" type="primary" @click="createShop">
        Добавить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, Card, Button, useToast } from "@beloyal/shared";
import { computed, ref } from "vue";
import { wait } from "@beloyal/shared";
import { useRouter } from "vue-router";
import { apiShopsCreate } from "~/api/shops";
import ShopForm from "~/components/Shops/ShopForm.vue";
import type { PartialShop } from "~/components/Shops/utils";

const isLoading = computed(() => wait.some([createShop]));
const router = useRouter();
const { showOk, showError } = useToast();
const model = ref<PartialShop>({ name: "", timeZone: undefined });

const createShop = async () => {
  if (!model.value.name) {
    return;
  }

  try {
    wait.start(createShop);
    await apiShopsCreate(model.value);
    showOk();
    router.push("/shops");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(createShop);
  }
};
</script>
