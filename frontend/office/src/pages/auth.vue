<template>
  <div :class="$style.auth">
    <Card :class="$style.form">
      <template v-if="!expiredAt">
        <Input :disabled="isLoading" placeholder="E-mail" v-model="email" />
        <Button :disabled="isLoading" @click="sendEmail"> Получить код </Button>
      </template>
      <template v-else>
        <Input disabled placeholder="E-mail" :model-value="email" />
        <Input :disabled="isLoading" placeholder="Код" v-model="code" />
        <Button :disabled="isLoading" @click="sendCode"> Отправить код </Button>
      </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import Button from "~/components/Button/Button.vue";
import Input from "~/components/Input/Input.vue";
import { self } from "~/composables/self";
import { apiAuthLogout, apiAuth, apiAuthConfirm } from "~/api/auth";
import { computed, ref } from "vue";
import { wait } from "~/composables/wait";
import { useRouter } from "vue-router";
import Card from "~/components/Card/Card.vue";

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
