<template>
  <template v-for="item in items">
    <div
      @click="isModel(item) ? unsetModel(item) : setModel(item)"
      :class="[
        $style.item,
        { [$style.active]: isModel(item), [$style.disabled]: disabled },
      ]"
    >
      {{ labelFn?.(item) || extract(item, labelPath) }}
    </div>
  </template>
</template>

<script setup lang="ts">
import { isDeepEqual } from "remeda";
import { computed } from "vue";
import { extract } from "~/components/MenuList/utils";

const {
  labelPath = "label",
  labelFn,
  valuePath,
  idPath,
  items,
  strategy = "click",
  withClear = false,
} = defineProps<{
  items: Array<any>;
  labelPath?: string;
  labelFn?: (_: any) => any;
  valuePath?: string;
  idPath?: string;
  disabled?: boolean;
  strategy?: "click" | "multiple";
  withClear?: boolean;
}>();

const model = defineModel<any>();
const arrModel = computed<any[]>(() => [].concat(model.value));
const comparePath = computed(() => idPath || valuePath);

const isModel = (v: unknown) => {
  const value = extract(v, comparePath.value);
  return arrModel.value.some((vv) =>
    isDeepEqual(extract(vv, comparePath.value), value)
  );
};

const setModel = (v: unknown) => {
  const value = extract(v, valuePath);

  if (strategy === "click") {
    model.value = value;
  } else if (strategy === "multiple") {
    model.value = arrModel.value.concat(value).filter((v) => v !== undefined);
  }
};

const unsetModel = (v: unknown) => {
  const value = extract(v, comparePath.value);

  if (withClear && strategy === "click") {
    model.value = null;
  } else if (strategy === "multiple") {
    model.value = arrModel.value
      .filter((vv) => !isDeepEqual(extract(vv, comparePath.value), value))
      .filter((v) => v !== undefined);
  }
};
</script>

<style module>
.item {
  padding: 12px;
  border-radius: 8px;
  user-select: none;
}

.item:not(:first-child) {
  margin-top: 4px;
}

.item.disabled {
  pointer-events: none;
}

.item:not(.disabled) {
  cursor: pointer;
}

.item:hover {
  background-color: var(--color-item-hover);
}

.item.active {
  background-color: var(--color-item-hover);
}
</style>
