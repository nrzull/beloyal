<template></template>

<script setup lang="ts">
import { useRouter } from "vue-router";
import { isAdmin } from "~/composables/self";

const router = useRouter();

if (isAdmin.value) {
  router.push("/white-companies");
} else {
  router.push("/company-accesses");
}
</script>
