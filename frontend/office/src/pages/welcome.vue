<template>
  <div :class="$style.welcome">
    <Card :class="$style.form">
      <NButton @click="$router.push('/welcome/company')">
        Создать компанию
      </NButton>
      <NButton
        :disabled="!invitations.length"
        @click="$router.push('/welcome/invitations')"
      >
        Посмотреть приглашения
      </NButton>
      <NButton type="primary" ghost @click="$router.push('/auth')">
        Сменить учетную запись
      </NButton>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import type { CompanyAccessInvitation } from "~/api/companyAccessInvitations/types";
import { apiSelfGetCompanyAccessInvitations } from "~/api/self";
import { NButton } from "naive-ui";
import Card from "~/components/Card/Card.vue";

const invitations = ref<CompanyAccessInvitation[]>([]);

apiSelfGetCompanyAccessInvitations().then((v) => (invitations.value = v.data));
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
