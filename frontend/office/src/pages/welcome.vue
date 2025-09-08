<template>
  <div :class="$style.welcome">
    <Card :class="$style.form">
      <NButton type="primary" @click="$router.push('/welcome/company')">
        Создать компанию
      </NButton>

      <NButton
        v-if="invitations.length"
        @click="$router.push('/welcome/invitations')"
      >
        <NSpace :size="8" align="center" justify="center">
          <div>Посмотреть приглашения</div>
          <NBadge type="info" :value="invitations.length" />
        </NSpace>
      </NButton>
      <NButton
        type="primary"
        :bordered="false"
        ghost
        @click="$router.push('/auth')"
      >
        Сменить учетную запись
      </NButton>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import type { CompanyAccessInvitation } from "~/api/companyAccessInvitations/types";
import { apiSelfGetCompanyAccessInvitations } from "~/api/self";
import { NButton, NBadge, NSpace } from "naive-ui";
import { Card } from "@beloyal/shared";

const invitations = ref<CompanyAccessInvitation[]>([]);

apiSelfGetCompanyAccessInvitations().then(
  (v) => (invitations.value = v.data.filter((vv) => vv.status === "Active"))
);
</script>

<style module>
.welcome {
  display: flex;
  justify-content: center;
  align-items: center;
  width: inherit;
  height: inherit;
}

.form {
  width: 375px;
  display: grid;
  gap: 12px;
}
</style>
