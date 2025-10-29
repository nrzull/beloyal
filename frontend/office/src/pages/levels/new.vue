<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Новый уровень</Heading>
    </template>

    <Card class="v-space-24">
      <LevelForm v-model="model" :loading="isLoading" />
      <Button type="primary" @click="createLevel" :disabled="isLoading">
        Создать
      </Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import LevelForm from "~/components/Levels/LevelForm.vue";
import { Card, useToast, wait } from "@beloyal/shared";
import Page from "~/components/Page.vue";
import { Heading, Button } from "@beloyal/shared";
import { computed, ref } from "vue";
import { newLevel } from "~/components/Levels/utils";
import { apiLevelsCreate } from "~/api/levels";
import { useRouter } from "vue-router";

const model = ref(newLevel());
const router = useRouter();
const { showOk, showError } = useToast();

const isLoading = computed(() => wait.some([createLevel]));

const createLevel = async () => {
  try {
    wait.start(createLevel);
    await apiLevelsCreate(model.value);
    showOk();
    router.push("/levels");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(createLevel);
  }
};
</script>
