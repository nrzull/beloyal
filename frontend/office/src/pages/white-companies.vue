<template>
  <Page>
    <template #top>
      <PageHeading>Белый список</PageHeading>
      <Button size="m" @click="$router.push('/white-companies/new')">
        Добавить компанию
      </Button>
    </template>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Button from "~/components/Button/Button.vue";
import { apiWhiteCompaniesGetAll } from "~/api/whiteCompanies";
import { ref } from "vue";
import type { WhiteCompany } from "~/api/whiteCompanies/types";

const whiteCompanies = ref<WhiteCompany[]>([]);

apiWhiteCompaniesGetAll().then((v) => (whiteCompanies.value = v.data));
</script>
