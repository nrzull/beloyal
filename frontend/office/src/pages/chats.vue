<template>
  <Page>
    <template #top>
      <Heading>Чаты</Heading>
    </template>

    <Messenger />
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading } from "@beloyal/shared";
import Messenger from "~/components/Chats/Messenger.vue";
</script>
