<template>
  <Page>
    <template #top>
      <PageHeading>Новая компания</PageHeading>
    </template>

    <Card :class="$style.form">
      <Input :disabled="isLoading" placeholder="ИНН" v-model="tin" />
      <NButton :disabled="isLoading" @click="createWhiteCompany">
        Добавить
      </NButton>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Card from "~/components/Card/Card.vue";
import Input from "~/components/Input/Input.vue";
import { NButton } from "naive-ui";
import { computed, ref } from "vue";
import { wait } from "~/composables/wait";
import { apiWhiteCompaniesCreate } from "~/api/whiteCompanies";
import { addMonths, formatISO } from "date-fns";
import { useRouter } from "vue-router";

const tin = ref();
const router = useRouter();
const isLoading = computed(() => wait.some([createWhiteCompany]));

const createWhiteCompany = async () => {
  if (!tin.value) {
    return;
  }

  try {
    wait.start(createWhiteCompany);

    await apiWhiteCompaniesCreate({
      tin: tin.value,
      expiredAt: formatISO(addMonths(new Date(), 1)),
    });

    router.push("/white-companies");
  } catch (e) {
    console.error(e);
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
