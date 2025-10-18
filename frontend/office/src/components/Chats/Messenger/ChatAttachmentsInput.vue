<template>
  <div>
    <NBadge @click="inputRef?.click()" type="info" :value="model.length">
      <Icon variant="attach" :class="$style.attach" />
    </NBadge>

    <input
      ref="input"
      type="file"
      multiple
      @input="handleFiles"
      :class="$style.input"
    />
  </div>
</template>

<script setup lang="ts">
import { NBadge } from "naive-ui";
import { Icon } from "@beloyal/shared";
import { useTemplateRef } from "vue";

defineProps<{ loading: boolean }>();

const model = defineModel<File[]>({ required: true });

const inputRef = useTemplateRef("input");

const handleFiles = (e: any) => {
  model.value = Array.from(e.target.files || []);
};
</script>

<style module>
.attach {
  cursor: pointer;
}

.attach:hover {
  color: var(--color-primary);
}

.input {
  position: absolute;
  width: 0;
  height: 0;
  z-index: -1;
  visibility: hidden;
  opacity: 0;
  left: -100vw;
  top: -100vh;
}
</style>
