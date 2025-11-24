<template>
  <Card :class="$style.authForm">
    <NThing>
      <template #header>
        <template v-if="expiredAt">
          <div>
            Введите
            <b :class="$style.accent">код для входа</b> в систему, который
            отправили вам в <b :class="$style.accent">мессенджер</b>
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
        placeholder="Код для входа"
        :maxlength="4"
      />
      <Space v-else vertical>
        <Input
          ref="phoneRef"
          placeholder="Номер телефона"
          v-model:value="phone"
        />

        <NAlert :show-icon="false" :bordered="false" type="info">
          <template #header>
            Для регистрации воспользуйтесь одним из&nbsp;ботов:
          </template>

          <Space vertical :size="[0, 12]">
            <template v-for="variant in variants">
              <a
                v-if="variant.type === 'telegram'"
                target="_blank"
                rel="nofollower"
                :style="{ color: 'var(--color-primary)' }"
                :href="`https://t.me/${variant.value}`"
              >
                <Space align="center" :size="[4, 0]">
                  <Icon variant="telegram" />
                  Telegram Bot
                </Space>
              </a>
            </template>
          </Space>
        </NAlert>
      </Space>
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
import { type LoginVariant } from "../api";
import { NAlert } from "naive-ui";
import Icon from "./Icon.vue";
import { useToast } from "../composables/useToast";

const { showError } = useToast();

const { profile, logoutFn, authFn, confirmFn, getVariantsFn } = defineProps<{
  profile: any;
  logoutFn: () => Promise<any>;
  authFn: (params: { phone: string }) => Promise<{ expiredAt: string }>;
  confirmFn: (params: { phone: string; code: string }) => Promise<any>;
  getVariantsFn: () => Promise<LoginVariant[]>;
}>();

if (profile) {
  logoutFn().finally(() => location.reload());
}

const router = useRouter();
const expiredAt = ref();
const code = ref<string>();
const phone = ref();
const variants = ref<LoginVariant[]>([]);

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
  } catch (e: any) {
    console.error(e);

    if (e?.response?.status === 404) {
      showError("Пользователь не найден");
    }
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

getVariantsFn().then((v) => (variants.value = v));
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
