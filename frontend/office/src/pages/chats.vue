<template>
  <Page>
    <template #top>
      <PageHeading>Чаты</PageHeading>
    </template>

    <Messenger />
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import Messenger from "~/components/Chats/Messenger.vue";
</script>
