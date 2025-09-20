<template>
  <MobileLayout v-if="done">
    <template #footer="{ footerClass }">
      <MobileFooter
        :class="footerClass"
        :items="[
          {
            to: `/${$route.params.messenger}`,
            title: 'Главная',
            iconVariant: 'home',
          },
          {
            to: `/${$route.params.messenger}/history`,
            title: 'История',
            iconVariant: 'history',
          },
        ]"
      />
    </template>
  </MobileLayout>
</template>

<script setup lang="ts">
import { MobileLayout, MobileFooter } from "@beloyal/shared";
import { ref } from "vue";
import { wsClient } from "~/api/clients";

const done = ref(false);
wsClient.start().then(() => (done.value = true));
</script>
