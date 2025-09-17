<template>
  <Card :class="$style.authForm">
    <template v-if="!expiredAt">
      <NInput
        ref="phoneRef"
        :disabled="isLoading"
        placeholder="Номер телефона"
        v-model:value="phone"
      />
      <NButton type="primary" :disabled="isLoading" @click="sendPhone">
        Получить код
      </NButton>
    </template>
    <template v-else>
      <NInput
        disabled
        ref="phoneRef"
        placeholder="Номер телефона"
        :value="phone"
      />
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
import { useTemplateRef } from "vue";
import { usePhoneMask } from "../composables/usePhoneMask";
import { unmaskPhone } from "../utils/phone";

const { profile, logoutFn, authFn, confirmFn } = defineProps<{
  profile: any;
  logoutFn: () => Promise<any>;
  authFn: (params: {
    phone: string;
  }) => Promise<{ code?: string; expiredAt: string }>;
  confirmFn: (params: { phone: string; code: string }) => Promise<any>;
}>();

if (profile) {
  logoutFn().finally(() => location.reload());
}

const router = useRouter();
const expiredAt = ref();
const code = ref();
const phone = ref();

const phoneRef = useTemplateRef("phoneRef");
usePhoneMask(computed(() => phoneRef.value?.inputElRef!));

const isLoading = computed(() => wait.some([sendPhone, sendCode]));

const sendPhone = async () => {
  const unmaskedPhone = unmaskPhone(phone.value);

  if (!unmaskedPhone) {
    return;
  }

  try {
    wait.start(sendPhone);

    const result = await authFn({ phone: unmaskedPhone });
    expiredAt.value = result.expiredAt;
    code.value = result.code;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(sendPhone);
  }
};

const sendCode = async () => {
  const unmaskedPhone = unmaskPhone(phone.value);

  if (!code.value || !unmaskedPhone) {
    return;
  }

  try {
    wait.start(sendCode);
    await confirmFn({ phone: unmaskedPhone, code: code.value });
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
