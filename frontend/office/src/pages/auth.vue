<template>
  <div :class="$style.auth">
    <div :class="$style.form">
      <template v-if="!expiredAt">
        <Input :disabled="isLoading" placeholder="E-mail" v-model="email" />
        <Button :disabled="isLoading || !email" @click="sendEmail">
          Получить код
        </Button>
      </template>
      <template v-else>
        <Input disabled placeholder="E-mail" :model-value="email" />
        <Input :disabled="isLoading" placeholder="Код" v-model="code" />
        <Button :disabled="isLoading || !code" @click="sendCode">
          Отправить код
        </Button>
      </template>
    </div>
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

if (self.value) {
  apiAuthLogout().finally(() => location.reload());
}

const router = useRouter();
const expiredAt = ref();
const code = ref();
const email = ref();

const isLoading = computed(() => wait.some([sendEmail, sendCode]));

const sendEmail = async () => {
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
  background-color: var(--color-white);
  padding: 32px 24px;
  border-radius: 8px;
  box-shadow: var(--box-shadow-card);
}
</style>
