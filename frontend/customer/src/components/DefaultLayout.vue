<template>
  <MobileLayout>
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
</script>
