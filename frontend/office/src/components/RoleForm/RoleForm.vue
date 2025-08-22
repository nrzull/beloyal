<template>
  <div class="v-space-16">
    <NInput :disabled="loading" placeholder="Название" v-model:value="name" />

    <div :class="$style.switch">
      <NSwitch
        :disabled="loading"
        :value="selectedPermissions.length == availablePermissions.length"
        @update:value="toggleAllPermissions"
      >
      </NSwitch>
      <div>Выбрать все</div>
    </div>

    <div class="v-space-24">
      <div
        v-for="(items, group) in groupedPermissions"
        :key="group"
        class="v-space-12"
      >
        <Heading size="m">
          {{ permissionGroupTitles[group] || group }}
        </Heading>

        <div class="v-space-12">
          <div :class="$style.switch" v-for="item in items" :key="item">
            <NSwitch
              :disabled="loading"
              :value="selectedPermissions.includes(item)"
              @update:value="togglePermission(item)"
            >
            </NSwitch>

            <div>{{ permissionTitles[item] || item }}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { NInput, NSwitch } from "naive-ui";
import { computed } from "vue";
import {
  permissionTitles,
  permissionGroupTitles,
} from "~/components/RoleForm/utils";

const { availablePermissions } = defineProps<{
  availablePermissions: string[];
  loading: boolean;
}>();

const name = defineModel<string>("name");

const selectedPermissions = defineModel<string[]>("selected-permissions", {
  required: true,
});

const groupedPermissions = computed(() =>
  Array.from(
    new Set(availablePermissions.map((p) => p.slice(0, p.lastIndexOf("_"))))
  ).reduce<Record<string, string[]>>((acc, value) => {
    acc[value] = availablePermissions.filter((v) => v.startsWith(value));
    return acc;
  }, {})
);

const togglePermission = (v: string) => {
  selectedPermissions.value = selectedPermissions.value.includes(v)
    ? selectedPermissions.value.filter((vv) => vv !== v)
    : selectedPermissions.value.concat([v]);
};

const toggleAllPermissions = () => {
  selectedPermissions.value =
    selectedPermissions.value.length < availablePermissions.length
      ? availablePermissions
      : [];
};
</script>

<style module>
.switch {
  display: flex;
  align-items: center;
  gap: 8px;
}
</style>
