<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Редактирование уровня</Heading>
    </template>

    <Card v-if="model" class="v-space-24">
      <LevelForm v-model="model" :loading="isLoading" />

      <Space vertical :size="[0, 12]">
        <Button block type="primary" @click="updateLevel" :disabled="isLoading">
          Сохранить
        </Button>

        <Button
          block
          type="error"
          ghost
          @click="deleteLevel"
          :disabled="isLoading"
        >
          Удалить
        </Button>
      </Space>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import LevelForm from "~/components/Levels/LevelForm.vue";
import {
  Card,
  Space,
  useToast,
  wait,
  waitable,
  type Level,
} from "@beloyal/shared";
import Page from "~/components/Page.vue";
import { Heading, Button } from "@beloyal/shared";
import { computed, ref } from "vue";
import { apiLevelsGet, apiLevelsUpdate, apiLevelsDelete } from "~/api/levels";
import { useRoute, useRouter } from "vue-router";

const model = ref<Level>();
const router = useRouter();
const route = useRoute();
const { showOk, showError } = useToast();
const levelId = route.params.id as string;

const isLoading = computed(() =>
  wait.some([apiLevelsGet, updateLevel, deleteLevel])
);

const updateLevel = async () => {
  try {
    wait.start(updateLevel);
    await apiLevelsUpdate(model.value!);
    showOk();
    router.push("/levels");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(updateLevel);
  }
};

const deleteLevel = async () => {
  try {
    wait.start(deleteLevel);
    await apiLevelsDelete(model.value!);
    showOk();
    router.push("/levels");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(deleteLevel);
  }
};

waitable(apiLevelsGet)({ id: levelId }).then((v) => (model.value = v.data));
</script>
