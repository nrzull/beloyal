<template>
  <Page :loading="isLoading">
    <template #top>
      <NBreadcrumb>
        <NBreadcrumbItem @click="$router.push('/customers')">
          Клиенты
        </NBreadcrumbItem>
        <NBreadcrumbItem>
          {{ customer?.user?.phone ? maskPhone(customer?.user?.phone) : "-" }}
        </NBreadcrumbItem>
      </NBreadcrumb>
    </template>

    <RouterView
      v-if="customer"
      :customer="customer"
      :loading="isLoading"
      v-slot="{ Component }"
    >
      <component :is="Component">
        <template #tabs>
          <NTabs :value="$route.path" @update:value="$router.push($event)">
            <NTab :name="`/customers/${customerId}/operations`">Операции</NTab>
            <NTab :name="`/customers/${customerId}/boosters`">Бустеры</NTab>
          </NTabs>
        </template>
      </component>
    </RouterView>
  </Page>
</template>

<script setup lang="ts">
import type { Customer } from "@beloyal/shared";
import { computed, ref } from "vue";
import { useRoute } from "vue-router";
import { apiCustomersGet } from "~/api/customers";
import Page from "~/components/Page.vue";
import { NTab, NTabs, NBreadcrumb, NBreadcrumbItem } from "naive-ui";
import { maskPhone, wait, waitable } from "@beloyal/shared";

const route = useRoute();
const customer = ref<Customer>();
const isLoading = computed(() => wait.some([apiCustomersGet]));

const customerId = route.params.id as string;

waitable(apiCustomersGet)({ customerId }).then(
  (v) => (customer.value = v.data)
);
</script>
