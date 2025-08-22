<template>
  <div class="v-space-24">
    <div :class="$style.top">
      <slot name="tabs"></slot>
      <NButton type="primary">Назначить сотрудника</NButton>
    </div>

    <Table
      :data="companyAccesses"
      :columns="[
        { key: 'email', title: 'E-mail' },
        { key: 'role', title: 'Роль' },
      ]"
    ></Table>
  </div>
</template>

<script setup lang="ts">
import { NButton } from "naive-ui";
import { ref } from "vue";
import type { CompanyAccess } from "~/api/companyAccesses/types";
import Table from "~/components/Table/Table.vue";

const companyAccesses = ref<CompanyAccess[]>([]);
</script>

<style module>
.companyAccesses {
}

.top {
  display: flex;
}
</style>
