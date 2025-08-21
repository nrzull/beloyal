<template>
  <div :class="$style.auth">
    <Card :class="$style.form">
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
  </div>
</template>

<script setup lang="ts">
import { self } from "~/composables/self";
import { apiAuthLogout, apiAuth, apiAuthConfirm } from "~/api/auth";
import { computed, ref } from "vue";
import { wait } from "~/composables/wait";
import { useRouter } from "vue-router";
import Card from "~/components/Card/Card.vue";
import { NInput, NButton } from "naive-ui";

if (self.value) {
  apiAuthLogout().finally(() => location.reload());
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

    const result = await apiAuth({ email: email.value });
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
    await apiAuthConfirm({ email: email.value, code: code.value });
    router.push("/");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(sendCode);
  }
};
</script>

<style module>
.auth {
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
