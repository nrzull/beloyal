<template>
  <div :class="$style.welcome">
    <Card :class="$style.form">
      <Button type="primary" @click="$router.push('/welcome/company')">
        Создать компанию
      </Button>

      <Button
        type="primary"
        :bordered="false"
        ghost
        @click="$router.push('/auth')"
      >
        Сменить учетную запись
      </Button>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { Card, Button } from "@beloyal/shared";
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
