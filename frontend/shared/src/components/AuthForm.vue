<template>
  <Card :class="$style.authForm">
    <template v-if="!expiredAt">
      <Input
        ref="phoneRef"
        :disabled="isLoading"
        placeholder="Номер телефона"
        v-model:value="phone"
      />
      <Button type="primary" :disabled="isLoading" @click="sendPhone">
        Получить код
      </Button>
    </template>
    <template v-else>
      <Input
        disabled
        ref="phoneRef"
        placeholder="Номер телефона"
        :value="phone"
      />
      <Input :disabled="isLoading" placeholder="Код" v-model:value="code" />
      <Button :disabled="isLoading" @click="sendCode" type="primary">
        Отправить код
      </Button>
    </template>
  </Card>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { wait } from "../composables/wait";
import { useRouter } from "vue-router";
import Card from "./Card.vue";
import Button from "./Button";
import Input from "./Input";
import { useTemplateRef } from "vue";
import { useMask } from "../composables/useMask";
import { unmaskPhone, phoneMask } from "../utils/phone";

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
useMask(
  computed(() => phoneRef.value?.inputElRef!),
  phoneMask
);

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
