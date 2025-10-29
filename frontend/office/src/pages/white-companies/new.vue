<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Новая компания</Heading>
    </template>

    <Card :class="$style.form">
      <Space vertical>
        <Input :disabled="isLoading" placeholder="ИНН" v-model:value="tin" />
        <NDatePicker
          v-model:formatted-value="expiredAt"
          placeholder="Укажите срок доступа"
        />
        <Button
          type="primary"
          block
          :disabled="isLoading"
          @click="createWhiteCompany"
        >
          Добавить
        </Button>
      </Space>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, Input, Button, Card, Space, useToast } from "@beloyal/shared";
import { computed, ref } from "vue";
import { wait } from "@beloyal/shared";
import { apiWhiteCompaniesCreate } from "~/api/whiteCompanies";
import { startOfDay, formatISO } from "date-fns";
import { useRouter } from "vue-router";
import { NDatePicker } from "naive-ui";

const tin = ref();
const expiredAt = ref();
const router = useRouter();
const isLoading = computed(() => wait.some([createWhiteCompany]));
const { showOk, showError } = useToast();

const createWhiteCompany = async () => {
  if (!tin.value || !expiredAt.value) {
    return;
  }

  try {
    wait.start(createWhiteCompany);

    await apiWhiteCompaniesCreate({
      tin: tin.value,
      expiredAt: formatISO(startOfDay(expiredAt.value)),
    });
    showOk();
    router.push("/white-companies");
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(createWhiteCompany);
  }
};
</script>

<style module>
.form {
  display: grid;
  gap: 12px;
}
</style>
