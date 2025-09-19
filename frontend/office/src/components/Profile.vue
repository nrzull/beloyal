<template>
  <NDropdown
    v-if="self"
    trigger="click"
    placement="right-start"
    @select="(v) => $router.push(v)"
    key-field="route"
    :options="[
      { type: 'route', label: 'Компании', route: '/self/company-accesses' },
      { type: 'route', label: 'Выйти', route: '/auth' },
    ]"
  >
    <div :class="$style.profile">
      <NSpace vertical :size="[0, 2]">
        <div :class="$style.team">
          {{
            self?.companyAccess?.company?.title || self?.adminAccess?.role?.name
          }}
        </div>
        <div :class="$style.phone">{{ maskPhone(self.user.phone!) }}</div>
      </NSpace>
      <Icon :class="$style.menuButton" variant="menu" />
    </div>
  </NDropdown>
</template>

<script setup lang="ts">
import { self } from "~/composables/self";
import { NDropdown, NSpace } from "naive-ui";
import { maskPhone, Icon } from "@beloyal/shared";
</script>

<style module>
.profile {
  margin-top: 12px;
  padding-top: 12px;
  padding-left: 36px;
  padding-right: 24px;
  padding-bottom: 12px;

  display: flex;
  align-items: center;
  gap: 4px;
  cursor: pointer;
  user-select: none;
}

.team {
  font-size: 14px;
  width: 124px;
  font-weight: 500;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}

.phone {
  font-size: 12px;
  font-weight: 500;
  opacity: 0.5;
}

.profile:hover .menuButton {
  color: var(--color-primary);
}

.menuButton {
  color: var(--color-grey-500);
}
</style>
