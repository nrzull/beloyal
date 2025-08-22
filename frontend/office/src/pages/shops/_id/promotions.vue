<template>
  <div class="v-space-24">
    <div :class="$style.top">
      <slot name="tabs"></slot>
      <NButton type="primary">Добавить акцию</NButton>
    </div>

    <Table
      :data="promotions"
      :columns="[
        { key: 'name', title: 'Название' },
        { key: 'createdAt', title: 'Добавлено' },
      ]"
    ></Table>
  </div>
</template>

<script setup lang="ts">
import { NButton } from "naive-ui";
import { ref } from "vue";
import Table from "~/components/Table/Table.vue";

const promotions = ref<any[]>([]);
</script>

<style module>
.top {
  display: flex;
}
</style>
