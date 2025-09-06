<template>
  <NConfigProvider
    :class="$style.provider"
    :theme-overrides="{
      common: {
        primaryColor: '#0073ff',
        primaryColorHover: '#1c82ff',
        primaryColorPressed: '#0073ff',
        primaryColorSuppl: '#0073ff',
      },
    }"
  >
    <RouterView v-if="self" />
  </NConfigProvider>
</template>

<script setup lang="ts">
import { NConfigProvider } from "naive-ui";

import {
  retrieveRawInitData,
  parseInitDataQuery,
} from "@telegram-apps/sdk-vue";
import { onMounted } from "vue";
import { useRoute } from "vue-router";
import { apiAuthLoginAsTelegramCustomer } from "~/api/auth";
import { customerToken } from "~/api/clients";
import { fetchSelf, self } from "~/composables/self";

const route = useRoute();

onMounted(async () => {
  const initData = retrieveRawInitData();
  if (initData) {
    const { hash } = parseInitDataQuery(initData);

    const {
      data: { token },
    } = await apiAuthLoginAsTelegramCustomer({
      query: initData,
      tgBotId: route.query.tgBotId as string,
      hash,
    });

    customerToken.value = token;
    await fetchSelf();
  }
});
</script>

<style module>
.provider {
  flex-flow: column;
  display: flex;
  height: inherit;
  width: inherit;
  overflow: hidden;
}
</style>
