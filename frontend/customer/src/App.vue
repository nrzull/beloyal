<template>
  <NConfigProvider :class="$style.provider" :theme-overrides="theme">
    <RouterView v-if="self" />
  </NConfigProvider>
</template>

<script setup lang="ts">
import { NConfigProvider } from "naive-ui";
import { theme } from "@beloyal/shared";

import {
  retrieveRawInitData,
  parseInitDataQuery,
  hapticFeedback,
} from "@telegram-apps/sdk-vue";
import { onMounted } from "vue";
import { useRoute } from "vue-router";
import { apiAuthLoginAsTelegramCustomer } from "~/api/auth";
import { authToken } from "~/api/clients";
import { publicToken } from "~/composables/publicToken";
import { fetchSelf, self } from "~/composables/self";
import { useMobileVh } from "@beloyal/shared";
import { wsClient } from "~/api/clients";

const route = useRoute();
useMobileVh({ variableName: "--vh1" });

const refresh = async () => {
  const initData = retrieveRawInitData();
  if (initData) {
    const { hash } = parseInitDataQuery(initData);

    const result = await apiAuthLoginAsTelegramCustomer({
      query: initData,
      tgBotId: route.query.tgBotId as string,
      hash,
    });

    authToken.value = result.data.authToken.token;
    publicToken.value = result.data.publicToken.token;
    await fetchSelf();

    if (hapticFeedback.isSupported()) {
      hapticFeedback.notificationOccurred("success");
    }
  }
};

onMounted(refresh);
wsClient.on("Refresh", refresh);
</script>

<style>
body {
  height: calc(var(--vh1) * 100);
}
</style>

<style module>
.provider {
  flex-flow: column;
  display: flex;
  height: inherit;
  width: inherit;
  overflow: hidden;
}
</style>
