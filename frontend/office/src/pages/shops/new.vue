<template>
  <Page>
    <template #top>
      <Heading>Новая торговая точка</Heading>
    </template>

    <Card class="v-space-12">
      <Input
        :disabled="isLoading"
        placeholder="Название"
        v-model:value="name"
      />

      <Button :disabled="isLoading" type="primary" @click="createShop">
        Добавить
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, Card, Button, Input } from "@beloyal/shared";
import { computed, ref } from "vue";
import { wait } from "@beloyal/shared";
import { useRouter } from "vue-router";
import { apiShopsCreate } from "~/api/shops";

const isLoading = computed(() => wait.some([createShop]));
const router = useRouter();
const name = ref<string>();

const createShop = async () => {
  if (!name.value) {
    return;
  }

  try {
    wait.start(createShop);
    await apiShopsCreate({ name: name.value });
    router.push("/shops");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(createShop);
  }
};
</script>
