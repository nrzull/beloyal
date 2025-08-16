<template>
  <div :class="$style.company">
    <Card :class="$style.form">
      <Input :disabled="isLoading" placeholder="ИНН" v-model="tin" />
      <Button :disabled="isLoading" @click="createCompany"
        >Создать компанию</Button
      >
      <Button
        :disabled="isLoading"
        type="link-primary"
        @click="$router.push('/welcome')"
      >
        Назад
      </Button>
    </Card>
  </div>
</template>

<script setup lang="ts">
import Card from "~/components/Card/Card.vue";
import Input from "~/components/Input/Input.vue";
import Button from "~/components/Button/Button.vue";
import { computed, ref } from "vue";
import { apiSelfCreateCompany } from "~/api/self";
import { wait } from "~/composables/wait";
import { useRouter } from "vue-router";

const tin = ref();
const router = useRouter();
const isLoading = computed(() => wait.some([createCompany]));

const createCompany = async () => {
  if (!tin.value) {
    return;
  }

  try {
    wait.start(createCompany);

    await apiSelfCreateCompany({
      cashbackPercent: 0,
      cashbackThreshold: 100,
      tin: tin.value,
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
