<template>
  <div :class="$style.welcome">
    <Card :class="$style.form">
      <NButton type="primary" @click="$router.push('/welcome/company')">
        Создать компанию
      </NButton>

      <NButton
        type="primary"
        :bordered="false"
        ghost
        @click="$router.push('/auth')"
      >
        Сменить учетную запись
      </NButton>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { NButton } from "naive-ui";
import { Card } from "@beloyal/shared";
</script>

<style module>
.welcome {
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
