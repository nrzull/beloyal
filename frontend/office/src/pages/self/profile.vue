<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>{{ t("userMenu.profileMenuTitle") }}</Heading>
    </template>

    <Card>
      <Space vertical>
        <div>{{ t("profilePage.setYourDataLabel") }}</div>
        <UserCredentialsForm v-model="model" />
        <Button
          :disabled="isLoading"
          type="primary"
          size="large"
          @click="update"
        >
          {{ t("profilePage.submitButton") }}
        </Button>
      </Space>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import { Button, Card, Heading, Space, useToast, wait } from "@beloyal/shared";
import { computed, ref } from "vue";
import { apiSelfUpdate } from "~/api/self";
import Page from "~/components/Page.vue";
import UserCredentialsForm from "~/components/UserCredentials/UserCredentialsForm.vue";
import { fetchSelf, self } from "~/composables/self";
import { useI18n } from "vue-i18n";
import type { CountryMessages } from "~/locales/types";

const { showOk, showError } = useToast();

const { t } = useI18n<{ message: CountryMessages }>();

const model = ref({
  firstName: self.value?.user?.firstName,
  lastName: self.value?.user?.lastName,
  middleName: self.value?.user?.middleName,
});

const isLoading = computed(() => wait.some([update]));

const update = async () => {
  try {
    wait.start(update);
    await apiSelfUpdate({
      firstName: model.value.firstName?.trim(),
      lastName: model.value.lastName?.trim(),
      middleName: model.value.middleName?.trim(),
    });
    await fetchSelf();
    showOk();
  } catch (e) {
    console.error(e);
    showError();
  } finally {
    wait.end(update);
  }
};
</script>
