<template>
  <Page>
    <template #top>
      <NBreadcrumb>
        <NBreadcrumbItem @click="$router.push('/telegram')">
          Telegram
        </NBreadcrumbItem>
        <NBreadcrumbItem>Новый бот</NBreadcrumbItem>
      </NBreadcrumb>
    </template>

    <Card class="v-space-12">
      <NInput :disabled="isLoading" placeholder="Токен" v-model:value="token" />
      <NButton type="primary" :disabled="isLoading" @click="createTgBot">
        Добавить
      </NButton>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { NBreadcrumb, NBreadcrumbItem, NButton, NInput } from "naive-ui";
import { Card } from "@beloyal/shared";
import { computed, ref } from "vue";
import { wait } from "@beloyal/shared";
import { apiMessengersCreateTgBot } from "~/api/messengers";

const emit = defineEmits<{ refresh: [] }>();

const token = ref();
const isLoading = computed(() => wait.some([createTgBot]));

const createTgBot = async () => {
  if (!token.value) {
    return;
  }

  try {
    wait.start(createTgBot);

    await apiMessengersCreateTgBot({ token: token.value });
    emit("refresh");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(createTgBot);
  }
};
</script>

<style module></style>
