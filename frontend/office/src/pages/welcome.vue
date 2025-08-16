<template>
  <div :class="$style.welcome">
    <Card :class="$style.form">
      <Button @click="$router.push('/welcome/company')">
        Создать компанию
      </Button>
      <Button
        :disabled="!invitations.length"
        @click="$router.push('/welcome/invitations')"
      >
        Посмотреть приглашения
      </Button>
      <Button type="link-primary" @click="$router.push('/auth')">
        Сменить учетную запись
      </Button>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import type { CompanyAccessInvitation } from "~/api/companyAccessInvitations/types";
import { apiSelfGetCompanyAccessInvitations } from "~/api/self";
import Button from "~/components/Button/Button.vue";
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
