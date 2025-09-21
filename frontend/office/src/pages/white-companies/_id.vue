<template>
  <Page>
    <template #top>
      <Heading>Редактирование доступа</Heading>
    </template>

    <Card v-if="whiteCompany" :class="$style.form">
      <Space vertical>
        <Input disabled placeholder="ИНН" v-model:value="whiteCompany.tin" />
        <NDatePicker
          :value="Date.parse(whiteCompany.expiredAt)"
          v-model:formatted-value="expiredAt"
          placeholder="Укажите срок доступа"
        />
        <Button
          type="primary"
          block
          :disabled="isLoading"
          @click="updateWhiteCompany"
        >
          Сохранить
        </Button>
      </Space>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import {
  Heading,
  Input,
  Button,
  Card,
  Space,
  type WhiteCompany,
} from "@beloyal/shared";
import { computed, ref } from "vue";
import { wait } from "@beloyal/shared";
import {
  apiWhiteCompaniesGet,
  apiWhiteCompaniesUpdate,
} from "~/api/whiteCompanies";
import { startOfDay, formatISO } from "date-fns";
import { useRoute, useRouter } from "vue-router";
import { NDatePicker } from "naive-ui";

const router = useRouter();
const route = useRoute();
const expiredAt = ref();
const isLoading = computed(() => wait.some([updateWhiteCompany]));

const whiteCompany = ref<WhiteCompany>();

const id = computed(() => route.params.id as string);

const updateWhiteCompany = async () => {
  if (!expiredAt.value) {
    return;
  }

  try {
    wait.start(updateWhiteCompany);

    await apiWhiteCompaniesUpdate({
      id: id.value,
      expiredAt: formatISO(startOfDay(expiredAt.value)),
    });

    router.push("/white-companies");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(updateWhiteCompany);
  }
};

apiWhiteCompaniesGet({ id: id.value }).then(
  (v) => (whiteCompany.value = v.data)
);
</script>

<style module>
.form {
  display: grid;
  gap: 12px;
}
</style>
