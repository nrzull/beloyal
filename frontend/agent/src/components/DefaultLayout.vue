<template>
  <MobileLayout>
    <template #footer="{ footerClass }">
      <MobileFooter
        :class="footerClass"
        :items="[
          { to: '/', title: 'Главная', iconVariant: 'home' },
          { to: '/operations', title: 'Операции', iconVariant: 'paymentCard' },
          { to: '/profile', title: 'Профиль', iconVariant: 'user' },
        ]"
      />
    </template>
  </MobileLayout>
</template>

<script setup lang="ts">
import { MobileLayout, MobileFooter } from "@beloyal/shared";
</script>
