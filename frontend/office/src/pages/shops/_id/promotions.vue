<template>
  <div class="v-space-24">
    <div :class="$style.top">
      <slot name="tabs"></slot>
      <Button type="primary">Добавить акцию</Button>
    </div>

    <Table
      :data="promotions"
      :columns="[
        { key: 'name', title: 'Название' },
        { key: 'createdAt', title: 'Создано' },
      ]"
    ></Table>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import Table from "~/components/Table/Table.vue";
import { Button } from "@beloyal/shared";

const promotions = ref<any[]>([]);
</script>

<style module>
.top {
  display: flex;
}
</style>
