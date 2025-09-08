<template>
  <div :class="$style.footer">
    <NTabs :value="$route.path" @update:value="$router.push($event)">
      <NTab name="/">
        <NSpace align="center">
          <Icon variant="home" />
          Главная
        </NSpace>
      </NTab>
      <NTab name="/profile">
        <NSpace align="center">
          <Icon variant="user" />
          Профиль
        </NSpace>
      </NTab>
    </NTabs>
  </div>
</template>

<script setup lang="ts">
import { NTabs, NTab, NSpace } from "naive-ui";
import { Icon } from "@beloyal/shared";
</script>

<style module>
.footer {
  padding: 0 24px;
  background-color: #fff;
  border-radius: 16px 16px 0 0;
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 0 auto;
}
</style>
