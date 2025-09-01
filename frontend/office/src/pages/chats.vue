<template>
  <Page>
    <template #top>
      <PageHeading>Чаты</PageHeading>
    </template>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import PageHeading from "~/components/PageHeading.vue";
import { apiChatsGetAll, apiChatsGetAllMessages } from "~/api/chats";

apiChatsGetAll().then((v) =>
  v.data.map((vv) => apiChatsGetAllMessages({ chatId: vv.id }))
);
</script>
