<template>
  <div :class="$style.auth">
    <AuthForm
      :auth-fn="apiAuth"
      :confirm-fn="apiAuthConfirm"
      :logout-fn="apiAuthLogout"
      :profile="false"
    />
  </div>
</template>

<script setup lang="ts">
import { apiAuthLogout, apiAuth, apiAuthConfirm } from "~/api/auth";
import { AuthForm } from "@beloyal/shared";
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
}
</style>
