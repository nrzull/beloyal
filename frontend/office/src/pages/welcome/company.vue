<template>
  <div :class="$style.company">
    <Card :class="$style.form">
      <NInput :disabled="isLoading" placeholder="ИНН" v-model:value="tin" />
      <NInput
        :disabled="isLoading"
        placeholder="Название"
        v-model:value="title"
      />
      <NButton type="primary" :disabled="isLoading" @click="createCompany">
        Создать компанию
      </NButton>
      <NButton
        :disabled="isLoading"
        type="primary"
        ghost
        @click="$router.push('/welcome')"
      >
        Назад
      </NButton>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { Card } from "@beloyal/shared";
import { NButton, NInput } from "naive-ui";
import { computed, ref } from "vue";
import { apiSelfCreateCompany } from "~/api/self";
import { wait } from "@beloyal/shared";
import { useRouter } from "vue-router";

const tin = ref();
const title = ref();
const router = useRouter();
const isLoading = computed(() => wait.some([createCompany]));

const createCompany = async () => {
  if (!tin.value || !title.value) {
    return;
  }

  try {
    wait.start(createCompany);

    await apiSelfCreateCompany({
      cashbackPercent: 10,
      cashbackThreshold: 25,
      bonusMultiplier: 10,
      tin: tin.value,
      title: title.value,
    });

    router.push("/");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(createCompany);
  }
};
</script>

<style module>
.company {
  display: flex;
  justify-content: center;
  align-items: center;
  width: inherit;
  height: inherit;
}

.form {
  width: 375px;
  display: grid;
  gap: 12px;
}
</style>
