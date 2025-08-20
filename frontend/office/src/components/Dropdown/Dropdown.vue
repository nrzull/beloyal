<template>
  <slot name="trigger" v-bind="{ setRef, open, setOpen }"></slot>

  <div
    v-if="open"
    ref="dropdownRef"
    :style="floatingStyles"
    :class="$style.dropdown"
  >
    <slot name="dropdown" v-bind="{ open, setOpen }"></slot>
  </div>
</template>

<script setup lang="ts">
import {
  flip,
  offset,
  useFloating,
  autoUpdate,
  size,
  type Placement,
} from "@floating-ui/vue";
import { ref, useTemplateRef } from "vue";
import { onClickOutside } from "@vueuse/core";

const { placement, width } = defineProps<{
  placement: Placement;
  width?: string;
}>();

const open = ref(false);
const triggerRef = ref();
const dropdownRef = useTemplateRef("dropdownRef");

onClickOutside(dropdownRef, () => (open.value = false));

const setRef = (v: any) => (triggerRef.value = v);
const setOpen = (v: boolean) => (open.value = v);

const { floatingStyles } = useFloating(triggerRef, dropdownRef, {
  open,
  placement,
  whileElementsMounted: autoUpdate,
  middleware: ref([
    offset(8),
    flip(),
    size({
      apply({ rects, elements }) {
        Object.assign(elements.floating.style, {
          width: width || `${rects.reference.width}px`,
        });
      },
    }),
  ]),
});

defineExpose({ setOpen });
</script>

<style module>
.dropdown {
  padding: 12px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.05);
  background-color: #fff;
  border-radius: 8px;
  border: 1px solid rgba(0, 0, 0, 0.05);
  z-index: 99;
}
</style>
