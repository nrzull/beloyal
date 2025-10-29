<template>
  <div :class="$style.company">
    <Card :class="$style.form">
      <CompanyForm :loading="isLoading" v-model="model" />
      <Button type="primary" :disabled="isLoading" @click="createCompany">
        Создать компанию
      </Button>
      <Button
        :disabled="isLoading"
        type="primary"
        ghost
        @click="$router.push('/welcome')"
      >
        Назад
      </Button>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { Button, Card, useToast } from "@beloyal/shared";
import { computed, ref } from "vue";
import { apiSelfCreateCompany } from "~/api/self";
import { wait } from "@beloyal/shared";
import { useRouter } from "vue-router";
import CompanyForm from "~/components/Company/CompanyForm.vue";

const model = ref({ tin: "", title: "" });

const router = useRouter();
const isLoading = computed(() => wait.some([createCompany]));
const { showOk, showError } = useToast();

const createCompany = async () => {
  if (!model.value.tin || !model.value.title) {
    return;
  }

  try {
    wait.start(createCompany);

    await apiSelfCreateCompany({
      ...model.value,
      cashbackPercent: 10,
      cashbackThreshold: 25,
      bonusMultiplier: 10,
      bonusLifetimeInDays: 180,
      levelLifetimeInDays: 14,
      timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone,
    });

    showOk();
    router.push("/");
  } catch (e) {
    console.error(e);
    showError();
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
