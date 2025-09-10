<template>
  <RouterView
    v-if="customer"
    :customer="customer"
    :public-token="publicToken"
    @refresh="fetchCustomer"
  />
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { apiCustomersGetByPublicToken, type Customer } from "~/api/customers";

const route = useRoute();
const router = useRouter();
const publicToken = computed(() => route.params.publicToken as string);

const customer = ref<Customer>();

const fetchCustomer = () =>
  apiCustomersGetByPublicToken({ publicToken: publicToken.value })
    .then((v) => (customer.value = v))
    .catch(() => router.push("/"));

fetchCustomer();
</script>
