<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Уровни</Heading>
      <Button size="medium" type="primary" @click="$router.push('/levels/new')"
        >Добавить уровень</Button
      >
    </template>

    <Table
      :data="levels"
      :columns="[
        { key: 'pointsCapacity', title: 'Очки' },
        { key: 'cashbackPercent', title: 'Кэшбэк' },
      ]"
      :route-fn="(v) => `/levels/${v.id}`"
    >
      <template #cashbackPercent="{ item }">
        {{ item.cashbackPercent }}%
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import { Heading, Button, type Level, wait, waitable } from "@beloyal/shared";
import { computed, ref } from "vue";
import { apiLevelsGetAll } from "~/api/levels";
import Page from "~/components/Page.vue";
import { Table } from "@beloyal/shared";

const levels = ref<Level[]>([]);
const isLoading = computed(() => wait.some([apiLevelsGetAll]));

waitable(apiLevelsGetAll)().then((v) => (levels.value = v.data));
</script>
