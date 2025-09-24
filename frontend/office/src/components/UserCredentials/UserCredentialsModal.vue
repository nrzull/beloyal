<template>
  <NModal
    @close="forcedHide = true"
    :class="$style.modal"
    :show="show"
    preset="card"
  >
    <template #header>Укажите ваши данные</template>

    <UserCredentialsForm v-model="model" />

    <template #footer>
      <Button :disabled="isLoading" type="primary" size="large" @click="update">
        Сохранить
      </Button>
    </template>
  </NModal>
</template>

<script setup lang="ts">
import { Button, wait } from "@beloyal/shared";
import { NModal } from "naive-ui";
import { computed, ref } from "vue";
import { self, fetchSelf } from "~/composables/self";
import { apiSelfUpdate } from "~/api/self";
import UserCredentialsForm from "~/components/UserCredentials/UserCredentialsForm.vue";

const model = ref({ firstName: "", lastName: "", middleName: "" });
const forcedHide = ref<boolean>(false);
const isLoading = computed(() => wait.some([update]));

const show = computed(() => {
  const partiallyFilled = [
    self.value?.user.firstName,
    self.value?.user.lastName,
    self.value?.user.middleName,
  ].some((v) => v);

  if (!self.value || partiallyFilled || forcedHide.value) {
    return false;
  }

  return true;
});

const update = async () => {
  try {
    wait.start(update);
    await apiSelfUpdate({
      firstName: model.value.firstName?.trim(),
      lastName: model.value.lastName?.trim(),
      middleName: model.value.middleName?.trim(),
    });
    await fetchSelf();
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(update);
  }
};
</script>

<style module>
.modal {
  max-width: 720px;
}
</style>
