<template>
  <Card :class="$style.authForm">
    <template v-if="!expiredAt">
      <NInput
        :disabled="isLoading"
        placeholder="E-mail"
        v-model:value="email"
      />
      <NButton type="primary" :disabled="isLoading" @click="sendEmail">
        Получить код
      </NButton>
    </template>
    <template v-else>
      <NInput disabled placeholder="E-mail" :value="email" />
      <NInput :disabled="isLoading" placeholder="Код" v-model:value="code" />
      <NButton :disabled="isLoading" @click="sendCode" type="primary">
        Отправить код
      </NButton>
    </template>
  </Card>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { wait } from "../composables/wait";
import { useRouter } from "vue-router";
import Card from "./Card.vue";
import { NInput, NButton } from "naive-ui";

const { profile, logoutFn, authFn, confirmFn } = defineProps<{
  profile: any;
  logoutFn: () => Promise<any>;
  authFn: (params: {
    email: string;
  }) => Promise<{ code?: string; expiredAt: string }>;
  confirmFn: (params: { email: string; code: string }) => Promise<any>;
}>();

if (profile) {
  logoutFn().finally(() => location.reload());
}

const router = useRouter();
const expiredAt = ref();
const code = ref();
const email = ref();

const isLoading = computed(() => wait.some([sendEmail, sendCode]));

const sendEmail = async () => {
  if (!email.value) {
    return;
  }

  try {
    wait.start(sendEmail);

    const result = await authFn({ email: email.value });
    expiredAt.value = result.expiredAt;
    code.value = result.code;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(sendEmail);
  }
};

const sendCode = async () => {
  if (!code.value) {
    return;
  }

  try {
    wait.start(sendCode);
    await confirmFn({ email: email.value, code: code.value });
    router.push("/");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(sendCode);
  }
};

defineExpose({ isLoading });
</script>

<style module>
.authForm {
  width: 375px;
  display: grid;
  gap: 12px;
}
</style>
