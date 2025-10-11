<template>
  <Page>
    <template #top>
      <Heading>Акции</Heading>
      <Button
        type="primary"
        size="medium"
        @click="$router.push('/promotions/new')"
      >
        Создать акцию
      </Button>
    </template>

    <Table
      :data="promotions"
      :columns="[
        { key: 'title', title: 'Название' },
        { key: 'variant', title: 'Тип' },
        { key: 'createdAt', title: 'Дата создания' },
      ]"
    >
    </Table>
  </Page>
</template>

<script setup lang="ts">
import Page from "~/components/Page.vue";
import { Heading, Button, Table, type Promotion } from "@beloyal/shared";
import { ref } from "vue";
import { apiPromotionsGetAll } from "~/api/promotions";

const promotions = ref<Promotion[]>([]);

apiPromotionsGetAll().then((v) => (promotions.value = v.data));
</script>
