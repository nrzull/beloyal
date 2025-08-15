<template>
  <div :class="$style.auth">
    <div :class="$style.form">
      <Input placeholder="E-mail" />
      <Button>Получить код</Button>
    </div>
  </div>
</template>

<script setup lang="ts">
import Button from "~/components/Button/Button.vue";
import Input from "~/components/Input/Input.vue";
import { self } from "~/composables/self";
import { apiAuthLogout } from "~/api/auth";

if (self.value) {
  apiAuthLogout().finally(() => location.reload());
}
</script>

<style module>
.auth {
  display: flex;
  justify-content: center;
  align-items: center;
  width: inherit;
  height: inherit;
}

.form {
  width: 375px;
  display: grid;
  gap: 12px;
  background-color: var(--color-white);
  padding: 32px 24px;
  border-radius: 8px;
  box-shadow: var(--box-shadow-card);
}
</style>
