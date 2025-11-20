<template>
  <Card :class="$style.authForm">
    <NThing>
      <template #header>
        <template v-if="expiredAt">
          <div>
            Введите
            <b :class="$style.accent">последние 4 цифры номера телефона</b>,
            который вам позвонит
          </div>
        </template>
        <template v-else>Войти в систему</template>
      </template>

      <template v-if="!expiredAt" #description>
        <div :class="$style.notice">
          Укажите номер телефона для входа в&nbsp;систему
        </div>
      </template>

      <template v-if="expiredAt" #footer>
        <Space vertical :size="[0, 8]">
          <Button
            :disabled="isLoading"
            @click="reset"
            text
            type="primary"
            size="tiny"
          >
            Ввести другой номер телефона
          </Button>

          <Button :disabled="isLoading" @click="sendCode" type="primary">
            Подтвердить
          </Button>
        </Space>
      </template>
      <template v-else #footer>
        <Button type="primary" :disabled="isLoading" @click="sendPhone">
          Получить код для входа
        </Button>
      </template>

      <Input
        v-if="expiredAt"
        :disabled="isLoading"
        v-model:value="code"
        placeholder="Последние 4 цифры"
        :maxlength="4"
      />
      <Input
        v-else
        ref="phoneRef"
        placeholder="Номер телефона"
        v-model:value="phone"
      />
    </NThing>
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
import { usePhoneMask } from "../composables/useMask";
import { unmaskPhone } from "../utils/phone";
import { NThing } from "naive-ui";
import Space from "./Space";

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
const code = ref<string>();
const phone = ref();

const reset = () => {
  phone.value = undefined;
  code.value = undefined;
  expiredAt.value = undefined;
};

const phoneRef = useTemplateRef("phoneRef");

usePhoneMask(
  computed(() => phoneRef.value?.inputElRef!),
  computed(() => "RU")
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

.notice {
  font-size: 13px;
  color: #999;
  line-height: 1;
}

.accent {
  color: var(--color-primary);
  line-height: 1.25;
  font-weight: 500;
}
</style>
