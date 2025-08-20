<template>
  <label
    :class="[
      $style.wrapper,
      {
        [$style.disabled]: disabled || readonly,
        [$style.withValue]: withValue,
        [$style.isFocused]: isFocused,
        [$style.hasError]: valid === false,
        [$style.withPlaceholder]: !!placeholder,
      },
    ]"
  >
    <div v-if="placeholder" :class="$style.placeholder">
      {{ placeholder }}
    </div>

    <slot name="input" v-bind="{ inputClass: $style.input, setFocused }">
      <input
        v-bind="$attrs"
        :readonly
        :disabled
        spellcheck="false"
        :class="$style.input"
        v-model="model"
        @focus="isFocused = true"
        @blur="isFocused = false"
      />
    </slot>

    <div @click.prevent :class="$style.actions">
      <Icon
        v-if="valid === false"
        :class="$style.errorIcon"
        variant="errorCircleFilled"
      />
      <Icon v-if="canInteract && withClear" variant="x" @click="model = null" />
      <slot name="actions"></slot>
    </div>
  </label>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import Icon from "~/components/Icon/Icon.vue";

defineOptions({ inheritAttrs: false });

const {
  disabled,
  readonly,
  valid = undefined,
} = defineProps<{
  disabled?: boolean;
  readonly?: boolean;
  placeholder?: string;
  withClear?: boolean;
  valid?: boolean;
}>();

const model = defineModel();
const canInteract = computed(() => !disabled && !readonly);

const isFocused = ref(false);

const setFocused = (v: boolean) => (isFocused.value = v);

const withValue = computed(() =>
  typeof model.value === "string"
    ? model.value.length
    : model.value !== undefined && model.value !== null
);
</script>

<style module>
.wrapper {
  display: flex;
  height: 40px;
  padding: 12px;
  align-items: center;
  gap: 8px;
  position: relative;
  border-radius: 8px;
  overflow: hidden;
  border: 1px solid rgba(0, 0, 0, 0.15);
  transition: border-color 0.2s ease;
}

.wrapper.isFocused {
  border-color: var(--color-primary);
}

.wrapper.hasError {
  border-color: var(--color-error);
}

.wrapper:not(.disabled) {
  cursor: text;
}

.input {
  all: unset;
  height: 100%;
  font-size: 15px;
  flex-grow: 1;
}

.placeholder {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  transition: all 0.2s ease;
  white-space: nowrap;
  user-select: none;
  pointer-events: none;
  font-size: 15px;
  opacity: 0.5;
}

:where(.withPlaceholder) .input {
  position: relative;
  top: 20%;
}

:where(.withValue, .isFocused) .placeholder {
  top: 15%;
  transform: translateY(-10%);
  font-size: 12px;
  font-weight: 500;
}

.actions {
  display: flex;
  gap: 4px;
}

.errorIcon {
  color: var(--color-error);
}
</style>
