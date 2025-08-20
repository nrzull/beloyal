<template>
  <Dropdown placement="bottom">
    <template #trigger="{ setOpen, setRef }">
      <Input
        :ref="setRef"
        v-bind="input"
        @click="setOpen(true)"
        readonly
        :model-value="inputValue"
      >
        <template
          v-if="menuList.strategy === 'multiple'"
          #input="{ inputClass }"
        >
          <div :class="[inputClass, $style.tags]">
            <template v-for="item in rawInputValue">
              <div :class="$style.tag">
                {{ item.label }}
                <Icon @click.stop="unsetModel(item.option)" variant="x" />
              </div>
            </template>
          </div>
        </template>
      </Input>
    </template>

    <template #dropdown="{ setOpen }">
      <MenuList
        v-bind="menuList"
        v-model="model"
        @update:model-value="
          menuList.strategy === 'multiple' ? undefined : setOpen(false)
        "
      />
    </template>
  </Dropdown>
</template>

<script setup lang="ts">
import Input from "~/components/Input/Input.vue";
import Dropdown from "~/components/Dropdown/Dropdown.vue";
import MenuList from "~/components/MenuList/MenuList.vue";
import { computed } from "vue";
import { extract } from "~/components/MenuList/utils";
import { isDeepEqual } from "remeda";
import Icon from "~/components/Icon/Icon.vue";

type MenuListProps = InstanceType<typeof MenuList>["$props"];

const { input, menuList } = defineProps<{
  input?: InstanceType<typeof Input>["$props"];
  menuList: Omit<MenuListProps, "modelValue">;
}>();

const model = defineModel<MenuListProps["modelValue"]>();
const arrModel = computed<any[]>(() => [].concat(model.value));

const comparePath = computed(() => menuList.idPath || menuList.valuePath);

const getModelFromOptions = (modelItem: unknown) =>
  menuList.items.find((item) =>
    isDeepEqual(
      extract(modelItem, comparePath.value),
      extract(item, comparePath.value)
    )
  );

const unsetModel = (v: unknown) => {
  const value = extract(v, comparePath.value);

  if (menuList.strategy === "click") {
    model.value = null;
  } else if (menuList.strategy === "multiple") {
    model.value = arrModel.value
      .filter((vv) => !isDeepEqual(extract(vv, comparePath.value), value))
      .filter((v) => v !== undefined);
  }
};

const rawInputValue = computed(() =>
  []
    .concat(model.value)
    .map((v) => {
      const option = getModelFromOptions(v);

      return {
        option: v,
        label:
          menuList.labelFn?.(option) || extract(option, menuList.labelPath),
      };
    })
    .filter((v) => v.option)
);

const inputValue = computed(() =>
  rawInputValue.value.map((v) => v.label).join(", ")
);
</script>

<style module>
.tags {
  position: relative;
  gap: 4px;
  top: 9px;
  display: flex;
  align-items: center;
  width: 0;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.tag {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  font-size: 12px;
  background-color: var(--color-primary);
  padding: 0px 4px;
  border-radius: 4px;
  color: #fff;
  cursor: pointer;
  user-select: none;
}
</style>
