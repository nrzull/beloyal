<template>
  <Page>
    <template #top>
      <Heading>Кэшбэк</Heading>
    </template>

    <Card>
      <Button type="primary" block size="large">Сохранить</Button>
    </Card>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading } from "@beloyal/shared";
import { Card, Button } from "@beloyal/shared";
</script>
