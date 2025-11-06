<template>
  <MobileLayout>
    <template #footer="{ footerClass }">
      <MobileFooter
        :class="footerClass"
        :items="[
          { to: '/', iconVariant: 'home' },
          { to: '/operations', iconVariant: 'paymentCard' },
          { to: '/profile', iconVariant: 'user' },
        ]"
      />
    </template>
  </MobileLayout>
</template>

<script setup lang="ts">
import { MobileLayout, MobileFooter } from "@beloyal/shared";
</script>
