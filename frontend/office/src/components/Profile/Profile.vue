<template>
  <NDropdown
    trigger="click"
    placement="right-start"
    @select="(v) => $router.push(v)"
    key-field="route"
    :options="[
      { type: 'route', label: 'Компании', route: '/self/company-accesses' },
      {
        type: 'route',
        label: 'Приглашения',
        route: '/self/company-access-invitations',
      },
      { type: 'route', label: 'Выйти', route: '/auth' },
    ]"
  >
    <div :class="$style.profile">
      <div>
        <div :class="$style.team">
          {{
            self?.companyAccess?.company?.tin || self?.adminAccess?.role?.name
          }}
        </div>
        <div :class="$style.email">{{ self?.user.email }}</div>
      </div>
    </div>
  </NDropdown>
</template>

<script setup lang="ts">
import { self } from "~/composables/self";
import { NDropdown } from "naive-ui";
</script>

<style module>
.profile {
  border-radius: 12px;
  padding: 12px 24px;
  margin: 12px 12px 0;
  display: flex;
  align-items: center;
  gap: 24px;
  cursor: pointer;
  background-color: #fafafa;
  user-select: none;
  box-shadow: 0 1px 0 rgba(0, 0, 0, 0.025);
}

.team {
  font-size: 14px;
  width: 124px;
  font-weight: 500;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}

.email {
  font-size: 12px;
  font-weight: 500;
  opacity: 0.5;
}
</style>
