<template>
  {{ $route.params.messenger }}

  {{ $route.query.tgBotId }}

  {{ parseInitDataQuery(retrieveRawInitData()!) }}
</template>

<script setup lang="ts">
import {
  retrieveRawInitData,
  parseInitDataQuery,
} from "@telegram-apps/sdk-vue";
import { onMounted } from "vue";
import { useRoute } from "vue-router";
import { apiAuthLoginAsTelegramCustomer } from "~/api/auth";

const route = useRoute();

onMounted(() => {
  const data = retrieveRawInitData();

  if (data) {
    const { hash } = parseInitDataQuery(data);

    apiAuthLoginAsTelegramCustomer({
      query: data,
      tgBotId: route.query.tgBotId as string,
      hash,
    });
  }
});
</script>
