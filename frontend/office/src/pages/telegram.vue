<template>
  <RouterView v-slot="{ Component }">
    <component
      v-if="!isLoading"
      :is="tgBot ? Component : NewPage"
      :bot="tgBot"
      @refresh="fetchBot"
    />
  </RouterView>
</template>

<script setup lang="ts">
import { apiMessengersGetTgBot } from "~/api/messengers";
import { computed, ref } from "vue";
import type { TgBot } from "~/api/messengers/types";
import NewPage from "~/pages/telegram/new.vue";
import { wait } from "@beloyal/shared";

const tgBot = ref<TgBot>();
const isLoading = computed(() => wait.some([fetchBot]));

const fetchBot = async () => {
  try {
    wait.start(fetchBot);

    const result = await apiMessengersGetTgBot();
    tgBot.value = result.data;
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(fetchBot);
  }
};

fetchBot();
</script>
