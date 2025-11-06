<template>
  <NDropdown
    v-if="self"
    trigger="click"
    placement="right-start"
    @select="(v) => $router.push(v)"
    key-field="route"
    :options="[
      {
        type: 'route',
        label: t('userMenu.profileMenuTitle'),
        route: '/self/profile',
      },
      {
        type: 'route',
        label: t('userMenu.companiesMenuTitle'),
        route: '/self/company-accesses',
      },
      { type: 'route', label: t('userMenu.exitMenuTitle'), route: '/auth' },
    ]"
  >
    <div :class="$style.profile">
      <Space vertical :size="[0, 2]">
        <div :class="$style.team">
          {{
            self?.companyAccess?.company?.title || self?.adminAccess?.role?.name
          }}
        </div>
        <div :class="$style.phone">{{ maskPhone(self.user.phone!) }}</div>
      </Space>
      <Icon :class="$style.menuButton" variant="menu" />
    </div>
  </NDropdown>
</template>

<script setup lang="ts">
import { self } from "~/composables/self";
import { NDropdown } from "naive-ui";
import { maskPhone, Space, Icon } from "@beloyal/shared";
import { useI18n } from "vue-i18n";
import type { CountryMessages } from "~/locales/types";

const { t } = useI18n<{ message: CountryMessages }>();
</script>

<style module>
.profile {
  margin-top: 12px;
  padding: 12px 24px;

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
