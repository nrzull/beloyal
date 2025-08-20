<template>
  <Page>
    <template #top>
      <PageHeading>Приглашения</PageHeading>
      <Button size="m" @click="$router.push('/company-access-invitations/new')">
        Создать приглашение
      </Button>
    </template>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Button from "~/components/Button/Button.vue";
</script>
