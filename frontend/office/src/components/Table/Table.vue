<template>
  <Card :with-padding="false" v-bind="card">
    <div :style="{ gridTemplateColumns: rowColumns }" :class="[$style.table]">
      <div :class="[$style.row]">
        <div
          v-for="column in columns"
          :style="{ alignItems: column.align, justifyContent: column.justify }"
          :class="[$style.cell, $style.headCell].concat(column.class as any ?? [])"
          :key="`${column.key}-head-cell`"
        >
          {{ column.title }}
        </div>
      </div>

      <div v-if="!data.length" :class="$style.infoRow">
        <template v-if="loading"> Загрузка... </template>
        <template v-else> Нет данных </template>
      </div>
      <template v-else v-for="item in data">
        <component
          :is="routeFn(item) ? 'router-link' : 'div'"
          :class="[
            $style.row,
            {
              [$style.clickable]: clickable,
              [$style.selected]: selectedFn(item),
            },
          ]"
          :to="routeFn(item)"
          @click="clickable ? $emit('row', item) : null"
        >
          <div
            v-for="column in columns"
            :style="{
              alignItems: column.align,
              justifyContent: column.justify,
            }"
            :class="[$style.cell].concat(column.class as any ?? [])"
            :key="`${column.key}-cell`"
          >
            <slot :name="column.key" v-bind="{ item }">
              {{ item[column.key] }}
            </slot>
          </div>
        </component>
      </template>
    </div>
  </Card>
</template>

<script setup lang="ts" generic="T extends Record<PropertyKey, unknown>">
import { computed } from "vue";
import type { RouteLocationRaw } from "vue-router";
import { Card } from "@beloyal/shared";

export type TableColumn = {
  key: string;
  title?: string;
  width?: string;
  class?: Array<string | Record<string, any>>;
  justify?: "flex-start" | "center" | "flex-end";
  align?: "flex-start" | "center" | "flex-end";
};

const {
  columns,
  data = [],
  selectedFn = () => false,
  routeFn = () => undefined,
} = defineProps<{
  columns: Array<TableColumn>;
  data?: Array<T>;
  routeFn?: (v: T) => RouteLocationRaw | undefined;
  selectedFn?: (v: T) => boolean;
  clickable?: boolean;
  loading?: boolean;
  card?: InstanceType<typeof Card>["$props"];
}>();

defineEmits<{ row: [_v: T] }>();

const rowColumns = computed(() =>
  columns.map((col) => col.width || "1fr").join(" ")
);
</script>

<style module>
.table {
  display: grid;
  overflow: auto;
  font-size: 14px;
  color: #111;
}

.infoRow {
  display: grid;
  grid-column: 1 / -1;
  padding: 16px;
  color: #999;
}

.row {
  display: grid;
  grid-column: 1 / -1;
  grid-template-columns: subgrid;
}

a.row {
  text-decoration: none;
  color: inherit;
  user-select: none;
}

a.row:hover,
.selected {
  background-color: #fafafa;
}

.clickable {
  cursor: pointer;
}

.clickable:hover {
  background-color: #fafafa;
}

.cell {
  padding: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
}

.cell:first-child {
  justify-content: flex-start;
}

.cell:last-child {
  justify-content: flex-end;
}

.headCell {
  font-weight: 500;
}
</style>
