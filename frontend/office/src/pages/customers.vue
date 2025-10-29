<template>
  <Page :loading="isLoading">
    <template #top>
      <Heading>Клиенты</Heading>
    </template>

    <Table
      :data="customers"
      :columns="[
        { key: 'phone', title: 'Номер телефона' },
        { key: 'createdAt', title: 'Дата регистрации' },
      ]"
      :route-fn="(v) => `/customers/${v.id}`"
    >
      <template #phone="{ item }">
        {{ item.user.phone ? maskPhone(item.user.phone) : "-" }}
      </template>

      <template #createdAt="{ item }">
        {{ formatDate(item.createdAt) }}
      </template>
    </Table>
  </Page>
</template>

<script setup lang="ts">
import { Heading, type Customer, Table, waitable, wait } from "@beloyal/shared";
import { computed, ref } from "vue";
import { apiCustomersGetAll } from "~/api/customers";
import Page from "~/components/Page.vue";
import { maskPhone } from "@beloyal/shared";
import { formatDate } from "@beloyal/shared";

const customers = ref<Customer[]>([]);

const isLoading = computed(() => wait.some([apiCustomersGetAll]));

waitable(apiCustomersGetAll)().then((v) => (customers.value = v.data));
</script>
