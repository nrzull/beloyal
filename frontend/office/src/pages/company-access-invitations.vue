<template>
  <Page>
    <template #top>
      <PageHeading>Приглашения</PageHeading>
      <NButton
        size="medium"
        @click="$router.push('/company-access-invitations/new')"
      >
        Создать приглашение
      </NButton>
    </template>
  </Page>
</template>

<script setup lang="ts">
import { NButton } from "naive-ui";
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
</script>
