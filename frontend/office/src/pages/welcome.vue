<template>welcome</template>

<script setup lang="ts">
import { ref } from "vue";
import type { CompanyAccessInvitation } from "~/api/companyAccessInvitations/types";
import { apiSelfGetCompanyAccessInvitations } from "~/api/self";

const invitations = ref<CompanyAccessInvitation[]>([]);

apiSelfGetCompanyAccessInvitations().then((v) => (invitations.value = v.data));
</script>
