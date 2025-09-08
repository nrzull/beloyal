<template>
  <div :class="$style.invitations">
    <Card :class="['v-space-24', $style.card]">
      <PageHeading>Мои приглашения</PageHeading>
      <Table
        :card="{ withPadding: false, withShadow: false }"
        :data="invitations"
        :columns="[
          { key: 'company', title: 'Компания' },
          { key: 'role', title: 'Роль' },
          { key: 'actions' },
        ]"
      >
        <template #company="{ item }">
          {{ item.company?.tin }}
        </template>

        <template #role="{ item }">
          {{ item.role?.name }}
        </template>

        <template #actions="{ item }">
          <NButton :disabled="isLoading" type="primary" @click="accept(item)">
            Принять
          </NButton>
        </template>
      </Table>

      <NButton ghost @click="$router.push('/welcome')">Назад</NButton>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { Card } from "@beloyal/shared";
import { computed, ref } from "vue";
import type { CompanyAccessInvitation } from "@beloyal/shared";
import {
  apiSelfGetCompanyAccessInvitations,
  apiSelfAcceptCompanyAccessInvitation,
} from "~/api/self";
import PageHeading from "~/components/PageHeading.vue";
import Table from "~/components/Table/Table.vue";
import { NButton } from "naive-ui";
import { wait } from "@beloyal/shared";
import { useRouter } from "vue-router";

const router = useRouter();
const invitations = ref<CompanyAccessInvitation[]>([]);
const isLoading = computed(() => wait.some([accept]));

apiSelfGetCompanyAccessInvitations().then(
  (v) => (invitations.value = v.data.filter((vv) => vv.status === "Active"))
);

const accept = async (v: CompanyAccessInvitation) => {
  try {
    wait.start(accept);

    await apiSelfAcceptCompanyAccessInvitation({
      companyAccessInvitationId: v.id,
    });

    router.push("/");
  } catch (e) {
    console.error(e);
  } finally {
    wait.end(accept);
  }
};
</script>

<style module>
.invitations {
  width: inherit;
  height: inherit;
  padding-top: 24px;
}

.card {
  max-width: 720px;
  margin: auto;
}
</style>
