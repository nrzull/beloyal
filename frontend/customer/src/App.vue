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
    <RouterView v-if="done" />
  </NConfigProvider>
</template>

<script setup lang="ts">
import { NConfigProvider } from "naive-ui";

import {
  retrieveRawInitData,
  parseInitDataQuery,
} from "@telegram-apps/sdk-vue";
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { apiAuthLoginAsTelegramCustomer } from "~/api/auth";

const route = useRoute();
const done = ref(false);

onMounted(() => {
  const data = retrieveRawInitData();

  if (data) {
    const { hash } = parseInitDataQuery(data);

    apiAuthLoginAsTelegramCustomer({
      query: data,
      tgBotId: route.query.tgBotId as string,
      hash,
    }).then(() => (done.value = true));
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
